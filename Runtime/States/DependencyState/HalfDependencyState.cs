using System;
using R3;

namespace TanitakaTech.StateVariable
{
    public class HalfDependencyState<T> : IStateObserver<T>, IStateSetter<T>, IDisposable
    {
        public void Set(T value) => _selfObservableState.Set(value);
        public T Read() => _resultSelector();
        public Observable<T> Observe() => _resultObservable;

        private readonly ObservableState<T> _selfObservableState;
        private readonly Observable<T> _resultObservable;
        private readonly Func<T> _resultSelector;

        public HalfDependencyState(ObservableState<T> selfObservableState, Observable<T> resultObservable,
            Func<T> resultSelector)
        {
            _selfObservableState = selfObservableState;
            _resultSelector = resultSelector;
            _resultObservable = resultObservable;
        }

        public void Dispose()
        {
            _selfObservableState.Dispose();
        }

        public static HalfDependencyState<Tr> Create<Tr>(Tr initialValue, Func<Tr, Tr> resultSelector)
        {
            var selfObservableState = new ObservableState<Tr>(initialValue);
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableState,
                resultObservable: selfObservableState.Observe().Select(resultSelector).DistinctUntilChanged(),
                resultSelector: () =>
                {
                    var result = resultSelector(selfObservableState.Read());
                    selfObservableState.SetWithoutNotify(result);
                    return result;
                });
        }

        public static HalfDependencyState<Tr> Create<T1, Tr>(Tr initialValue, Observable<T1> source1, Func<Tr, T1, Tr> resultSelector)
        {
            var selfObservableState = new ObservableState<Tr>(initialValue);
            T1 source1BeforeValue = default;
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableState,
                resultObservable: Observable.CombineLatest(
                    selfObservableState.Observe(),
                    source1.Do(v => source1BeforeValue = v),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () =>
                {
                    var result = resultSelector(selfObservableState.Read(), source1BeforeValue);
                    selfObservableState.SetWithoutNotify(result);
                    return result;
                });
        }

        public static HalfDependencyState<Tr> Create<T1, T2, Tr>(Tr initialValue, Observable<T1> source1, Observable<T2> source2, Func<Tr, T1, T2, Tr> resultSelector)
        {
            var selfObservableState = new ObservableState<Tr>(initialValue);
            T1 source1BeforeValue = default;
            T2 source2BeforeValue = default;
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableState,
                resultObservable: Observable.CombineLatest(
                    selfObservableState.Observe(),
                    source1.Do(v => source1BeforeValue = v),
                    source2.Do(v => source2BeforeValue = v),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () =>
                {
                    var result = resultSelector(selfObservableState.Read(), source1BeforeValue, source2BeforeValue);
                    selfObservableState.SetWithoutNotify(result);
                    return result;
                });
        }

        public static HalfDependencyState<Tr> Create<T1, T2, T3, Tr>(Tr initialValue, Observable<T1> source1, Observable<T2> source2, Observable<T3> source3, Func<Tr, T1, T2, T3, Tr> resultSelector)
        {
            var selfObservableState = new ObservableState<Tr>(initialValue);
            T1 source1BeforeValue = default;
            T2 source2BeforeValue = default;
            T3 source3BeforeValue = default;
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableState,
                resultObservable: Observable.CombineLatest(
                    selfObservableState.Observe(),
                    source1.Do(v => source1BeforeValue = v),
                    source2.Do(v => source2BeforeValue = v),
                    source3.Do(v => source3BeforeValue = v),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () =>
                {
                    var result = resultSelector(selfObservableState.Read(), source1BeforeValue, source2BeforeValue, source3BeforeValue);
                    selfObservableState.SetWithoutNotify(result);
                    return result;
                });
        }

        public static HalfDependencyState<Tr> Create<T1, Tr>(Tr initialValue, IStateObserver<T1> source1, Func<Tr, T1, Tr> resultSelector)
        {
            var selfObservableState = new ObservableState<Tr>(initialValue);
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableState,
                resultObservable: Observable.CombineLatest(
                    selfObservableState.Observe(), source1.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () =>
                {
                    var result = resultSelector(selfObservableState.Read(), source1.Read());
                    selfObservableState.SetWithoutNotify(result);
                    return result;
                });
        }

        public static HalfDependencyState<Tr> Create<T1, T2, Tr>(Tr initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, Func<Tr, T1, T2, Tr> resultSelector)
        {
            var selfObservableState = new ObservableState<Tr>(initialValue);
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableState,
                resultObservable: Observable.CombineLatest(
                    selfObservableState.Observe(), source1.Observe(), source2.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () =>
                {
                    var result = resultSelector(selfObservableState.Read(), source1.Read(), source2.Read());
                    selfObservableState.SetWithoutNotify(result);
                    return result;
                });
        }

        public static HalfDependencyState<Tr> Create<T1, T2, T3, Tr>(Tr initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, Func<Tr, T1, T2, T3, Tr> resultSelector)
        {
            var selfObservableState = new ObservableState<Tr>(initialValue);
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableState,
                resultObservable: Observable.CombineLatest(
                    selfObservableState.Observe(), source1.Observe(), source2.Observe(), source3.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () =>
                {
                    var result = resultSelector(selfObservableState.Read(), source1.Read(), source2.Read(), source3.Read());
                    selfObservableState.SetWithoutNotify(result);
                    return result;
                });
        }

        public static HalfDependencyState<Tr> Create<T1, T2, T3, T4, Tr>(Tr initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, Func<Tr, T1, T2, T3, T4, Tr> resultSelector)
        {
            var selfObservableState = new ObservableState<Tr>(initialValue);
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableState,
                resultObservable: Observable.CombineLatest(
                    selfObservableState.Observe(), source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () =>
                {
                    var result = resultSelector(selfObservableState.Read(), source1.Read(), source2.Read(), source3.Read(), source4.Read());
                    selfObservableState.SetWithoutNotify(result);
                    return result;
                });
        }

        public static HalfDependencyState<Tr> Create<T1, T2, T3, T4, T5, Tr>(Tr initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, Func<Tr, T1, T2, T3, T4, T5, Tr> resultSelector)
        {
            var selfObservableState = new ObservableState<Tr>(initialValue);
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableState,
                resultObservable: Observable.CombineLatest(
                    selfObservableState.Observe(), source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () =>
                {
                    var result = resultSelector(selfObservableState.Read(), source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read());
                    selfObservableState.SetWithoutNotify(result);
                    return result;
                });
        }

        public static HalfDependencyState<Tr> Create<T1, T2, T3, T4, T5, T6, Tr>(Tr initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, IStateObserver<T6> source6, Func<Tr, T1, T2, T3, T4, T5, T6, Tr> resultSelector)
        {
            var selfObservableState = new ObservableState<Tr>(initialValue);
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableState,
                resultObservable: Observable.CombineLatest(
                    selfObservableState.Observe(), source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () =>
                {
                    var result = resultSelector(selfObservableState.Read(), source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read());
                    selfObservableState.SetWithoutNotify(result);
                    return result;
                });
        }

        public static HalfDependencyState<Tr> Create<T1, T2, T3, T4, T5, T6, T7, Tr>(Tr initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, IStateObserver<T6> source6, IStateObserver<T7> source7, Func<Tr, T1, T2, T3, T4, T5, T6, T7, Tr> resultSelector)
        {
            var selfObservableState = new ObservableState<Tr>(initialValue);
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableState,
                resultObservable: Observable.CombineLatest(
                    selfObservableState.Observe(), source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(), source7.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () =>
                {
                    var result = resultSelector(selfObservableState.Read(), source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read(), source7.Read());
                    selfObservableState.SetWithoutNotify(result);
                    return result;
                });
        }
    }
}