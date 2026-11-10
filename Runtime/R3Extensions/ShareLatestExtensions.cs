#nullable enable
using System;
using System.Threading;
using R3;

namespace TanitakaTech.StateVariable.R3Extensions
{
    public static class ShareLatestExtensions
    {
        /// <summary>
        /// 元の Observable への購読を 1 本にまとめ、購読者で共有する。
        /// <list type="bullet">
        /// <item>最初の購読で元の Observable につなぎ、購読が 0 本になったら切る</item>
        /// <item>つながっている間は直近の値を持ち、後から購読した側にもその値をすぐ渡す</item>
        /// <item>切れたら持っている値を捨てるので、つなぎ直したときに古い値は流れない</item>
        /// <item>元の Observable が完了したら、次の購読でつなぎ直す</item>
        /// <item>同じ値が続いても間引かない（毎回流す）</item>
        /// </list>
        /// </summary>
        public static Observable<T> ShareLatest<T>(this Observable<T> source)
        {
            return source as ShareLatestObservable<T> ?? new ShareLatestObservable<T>(source);
        }
    }

    internal sealed class ShareLatestObservable<T> : Observable<T>
    {
        private readonly Observable<T> _source;
        private readonly object _gate = new object();
        private Connection? _connection;

        public ShareLatestObservable(Observable<T> source)
        {
            _source = source;
        }

        protected override IDisposable SubscribeCore(Observer<T> observer)
        {
            Connection connection;
            bool isFirst;
            lock (_gate)
            {
                isFirst = _connection == null;
                _connection ??= new Connection(this);
                connection = _connection;
                connection.RefCount++;
            }

            if (isFirst)
            {
                // NOTE: つなぐ時点ではまだ誰にも値を渡さず、直近の値として持つだけにする。
                //       ReactiveProperty は購読時に今の値を出してから購読者を登録するため、その最中に購読者が元を書き換えると取りこぼす。
                //       購読者に渡すのは元への登録が済んだ後にして、書き換えた値も確実に受け取れるようにする
                connection.Connect(_source);
            }

            if (connection.TryGetCompleted(out var completedResult))
            {
                // NOTE: つないだ時点で元が完了した（Observable.Return など）。値と完了をそのまま渡して終わる
                if (connection.TryGetLatest(out var lastValue))
                {
                    observer.OnNext(lastValue);
                }
                observer.OnCompleted(completedResult);
                Release(connection);
                return Disposable.Empty;
            }

            // NOTE: 先に Subject を購読してから直近の値を渡す。値を受け取った購読者が同期的に元を書き換えても、その後の値を取りこぼさないため
            var subjectSubscription = connection.Subject.Subscribe(new ForwardObserver(observer));
            var subscription = new Subscription(this, connection, subjectSubscription);
            if (connection.TryGetLatest(out var latest))
            {
                observer.OnNext(latest);
            }

            return subscription;
        }

        private void Release(Connection connection)
        {
            var disconnect = false;
            lock (_gate)
            {
                connection.RefCount--;
                if (connection.RefCount == 0 && _connection == connection)
                {
                    _connection = null;
                    disconnect = true;
                }
            }

            if (disconnect)
            {
                connection.Dispose();
            }
        }

        private void OnSourceCompleted(Connection connection, Result result)
        {
            lock (_gate)
            {
                if (_connection == connection)
                {
                    _connection = null;
                }
            }

            connection.Complete(result);
        }

        private sealed class Connection : IDisposable
        {
            public readonly Subject<T> Subject = new Subject<T>();
            public int RefCount;

            private readonly ShareLatestObservable<T> _parent;
            private readonly SingleAssignmentDisposable _sourceSubscription = new SingleAssignmentDisposable();
            private bool _hasValue;
            private T _latest = default!;
            private Result? _completedResult;

            public Connection(ShareLatestObservable<T> parent)
            {
                _parent = parent;
            }

            public void Connect(Observable<T> source)
            {
                // NOTE: 購読中に 0 本になって Dispose 済みなら、代入した時点で購読が破棄される
                _sourceSubscription.Disposable = source.Subscribe(new SourceObserver(this));
            }

            public bool TryGetLatest(out T latest)
            {
                latest = _latest;
                return _hasValue;
            }

            public bool TryGetCompleted(out Result result)
            {
                result = _completedResult.GetValueOrDefault();
                return _completedResult.HasValue;
            }

            public void Complete(Result result)
            {
                // NOTE: つないだ直後に完了した場合も、最初の購読者に値を渡せるよう直近の値は残す
                _completedResult = result;
                _sourceSubscription.Dispose();
                Subject.OnCompleted(result);
            }

            public void Dispose()
            {
                _hasValue = false;
                _latest = default!;
                _sourceSubscription.Dispose();
                Subject.Dispose();
            }

            private sealed class SourceObserver : Observer<T>
            {
                private readonly Connection _connection;

                public SourceObserver(Connection connection)
                {
                    _connection = connection;
                }

                protected override void OnNextCore(T value)
                {
                    _connection._latest = value;
                    _connection._hasValue = true;
                    _connection.Subject.OnNext(value);
                }

                protected override void OnErrorResumeCore(Exception error)
                {
                    _connection.Subject.OnErrorResume(error);
                }

                protected override void OnCompletedCore(Result result)
                {
                    _connection._parent.OnSourceCompleted(_connection, result);
                }
            }
        }

        private sealed class ForwardObserver : Observer<T>
        {
            private readonly Observer<T> _observer;

            public ForwardObserver(Observer<T> observer)
            {
                _observer = observer;
            }

            protected override void OnNextCore(T value) => _observer.OnNext(value);
            protected override void OnErrorResumeCore(Exception error) => _observer.OnErrorResume(error);
            protected override void OnCompletedCore(Result result) => _observer.OnCompleted(result);
        }

        private sealed class Subscription : IDisposable
        {
            private readonly ShareLatestObservable<T> _parent;
            private readonly Connection _connection;
            private readonly IDisposable _subjectSubscription;
            private int _disposed;

            public Subscription(ShareLatestObservable<T> parent, Connection connection, IDisposable subjectSubscription)
            {
                _parent = parent;
                _connection = connection;
                _subjectSubscription = subjectSubscription;
            }

            public void Dispose()
            {
                if (Interlocked.Exchange(ref _disposed, 1) == 1) return;
                _subjectSubscription.Dispose();
                _parent.Release(_connection);
            }
        }
    }
}
