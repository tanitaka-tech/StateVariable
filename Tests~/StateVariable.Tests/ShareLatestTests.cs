using System.Collections.Generic;
using R3;
using TanitakaTech.StateVariable;
using TanitakaTech.StateVariable.R3Extensions;
using Xunit;

namespace StateVariable.Tests
{
    public class ShareLatestTests
    {
        [Fact]
        public void 購読が何本あっても元の計算は1回で後から購読した側にも直近の値が届く()
        {
            var source = new ReactiveProperty<int>(1);
            var selectCount = 0;
            var shared = source.Select(x => { selectCount++; return x * 10; }).ShareLatest();

            var first = new List<int>();
            var late = new List<int>();
            using var s1 = shared.Subscribe(first.Add);
            using var s2 = shared.Subscribe(late.Add);

            Assert.Equal(new[] { 10 }, first);
            Assert.Equal(new[] { 10 }, late);
            Assert.Equal(1, selectCount);

            source.Value = 2;
            Assert.Equal(new[] { 10, 20 }, first);
            Assert.Equal(new[] { 10, 20 }, late);
            Assert.Equal(2, selectCount);
        }

        [Fact]
        public void 購読が0本になったら切れてつなぎ直しても古い値は流れない()
        {
            var source = new ReactiveProperty<int>(1);
            var subscribeCount = 0;
            var shared = Observable.Defer(() => { subscribeCount++; return source.AsObservable(); }).ShareLatest();

            shared.Subscribe(_ => { }).Dispose();
            source.Value = 5;

            var values = new List<int>();
            using var s = shared.Subscribe(values.Add);

            Assert.Equal(new[] { 5 }, values);
            Assert.Equal(2, subscribeCount);
        }

        [Fact]
        public void 同じ値が続いても間引かない()
        {
            var subject = new Subject<Unit>();
            var shared = subject.ShareLatest();
            var count = 0;
            using var s = shared.Subscribe(_ => count++);

            subject.OnNext(Unit.Default);
            subject.OnNext(Unit.Default);

            Assert.Equal(2, count);
        }

        [Fact]
        public void 値が来る前に購読した側には何も渡さない()
        {
            var subject = new Subject<int>();
            var shared = subject.ShareLatest();
            using var s1 = shared.Subscribe(_ => { });
            var values = new List<int>();
            using var s2 = shared.Subscribe(values.Add);

            Assert.Empty(values);
        }

        [Fact]
        public void 元が完了したら次の購読でつなぎ直す()
        {
            var subscribeCount = 0;
            var shared = Observable.Defer(() => { subscribeCount++; return Observable.Return(subscribeCount); }).ShareLatest();

            var first = new List<int>();
            var firstCompleted = false;
            shared.Subscribe(first.Add, _ => firstCompleted = true);
            var second = new List<int>();
            shared.Subscribe(second.Add);

            Assert.Equal(new[] { 1 }, first);
            Assert.True(firstCompleted);
            Assert.Equal(new[] { 2 }, second);
        }

        [Fact]
        public void 最初の値を受け取った中で購読をやめたら元への購読も切れる()
        {
            var source = new ReactiveProperty<int>(1);
            var shared = source.ShareLatest();

            var values = new List<int>();
            shared.Take(1).Subscribe(values.Add);
            source.Value = 2;

            Assert.Equal(new[] { 1 }, values);
            // NOTE: 切れていれば、次の購読でつなぎ直して今の値を受け取る
            var next = new List<int>();
            using var s = shared.Subscribe(next.Add);
            Assert.Equal(new[] { 2 }, next);
        }

        [Fact]
        public void 値を受け取った購読者が元を書き換えても後から購読した側は最新の値で終わる()
        {
            var source = new ReactiveProperty<int>(0);
            var shared = source.ShareLatest();
            using var s1 = shared.Subscribe(v => { if (v == 0) source.Value = 1; });

            var values = new List<int>();
            using var s2 = shared.Subscribe(values.Add);

            Assert.Equal(1, values[^1]);
        }

        [Fact]
        public void 最初の値を受け取った中で元を書き換えたら最初の購読者もその値を受け取る()
        {
            var source = new ReactiveProperty<int>(0);
            var shared = source.ShareLatest();
            var values = new List<int>();
            using var s1 = shared.Subscribe(v =>
            {
                values.Add(v);
                if (v == 0) source.Value = 1;
            });

            Assert.Equal(new[] { 0, 1 }, values);
        }

        [Fact]
        public void DependencyStateを入れ子にしても値が伝わる()
        {
            var a = new ObservableState<int>(1);
            var inner = DependencyState<int>.Create(a, x => x * 2);
            var outer = DependencyState<int>.Create(inner, x => x + 1);

            var innerValues = new List<int>();
            var outerValues = new List<int>();
            using var s1 = inner.Observe().Subscribe(innerValues.Add);
            using var s2 = outer.Observe().Subscribe(outerValues.Add);

            a.Set(2);
            Assert.Equal(new[] { 2, 4 }, innerValues);
            Assert.Equal(new[] { 3, 5 }, outerValues);
            Assert.Equal(5, outer.Read());
        }

        [Fact]
        public void DependencyStateは購読の数だけ計算し直さない()
        {
            var a = new ObservableState<int>(1);
            var b = new ObservableState<int>(10);
            var calcCount = 0;
            var state = DependencyState<int>.Create(a, b, (x, y) => { calcCount++; return x + y; });

            var values1 = new List<int>();
            var values2 = new List<int>();
            using var s1 = state.Observe().Subscribe(values1.Add);
            using var s2 = state.Observe().Subscribe(values2.Add);
            Assert.Equal(1, calcCount);

            a.Set(2);
            Assert.Equal(2, calcCount);
            Assert.Equal(new[] { 11, 12 }, values1);
            Assert.Equal(new[] { 11, 12 }, values2);

            // NOTE: Read() は共有せず、その場で計算する
            Assert.Equal(12, state.Read());
            Assert.Equal(3, calcCount);
        }

        [Fact]
        public void DependencyStateは購読がなければ計算しない()
        {
            var a = new ObservableState<int>(1);
            var calcCount = 0;
            var state = DependencyState<int>.Create(a, x => { calcCount++; return x; });

            a.Set(2);
            Assert.Equal(0, calcCount);

            state.Observe().Subscribe(_ => { }).Dispose();
            a.Set(3);
            Assert.Equal(1, calcCount);
        }

        [Fact]
        public void HalfDependencyStateも購読の数だけ計算し直さない()
        {
            var source = new ObservableState<int>(1);
            var calcCount = 0;
            using var state = HalfDependencyState<int>.Create(0, source.Observe(), (self, x) => { calcCount++; return self + x; });

            var values = new List<int>();
            using var s1 = state.Observe().Subscribe(_ => { });
            using var s2 = state.Observe().Subscribe(values.Add);
            Assert.Equal(1, calcCount);

            state.Set(5);
            Assert.Equal(2, calcCount);
            Assert.Equal(new[] { 1, 6 }, values);
        }

        [Fact]
        public void ToStateObserverは元への購読を1本にまとめる()
        {
            var source = new ReactiveProperty<int>(1);
            var selectCount = 0;
            var observer = source.Select(x => { selectCount++; return x; }).ToStateObserver(out var disposable);

            var values = new List<int>();
            using var s1 = observer.Observe().Subscribe(values.Add);
            using var s2 = observer.Observe().Subscribe(_ => { });
            Assert.Equal(1, selectCount);
            Assert.Equal(new[] { 1 }, values);

            source.Value = 2;
            Assert.Equal(2, selectCount);
            Assert.Equal(2, observer.Read());

            disposable.Dispose();
        }
    }
}
