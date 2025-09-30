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
            var selfObservableVariable = new ObservableState<Tr>(initialValue);
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableVariable,
                resultObservable: selfObservableVariable.Observe().Select(resultSelector).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read())
            );
        }

        public static HalfDependencyState<Tr> Create<T1, Tr>(Tr initialValue, IStateObserver<T1> source1, Func<Tr, T1, Tr> resultSelector)
        {
            var selfObservableVariable = new ObservableState<Tr>(initialValue);
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableVariable,
                resultObservable: Observable.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read())
            );
        }

        public static HalfDependencyState<Tr> Create<T1, T2, Tr>(Tr initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, Func<Tr, T1, T2, Tr> resultSelector)
        {
            var selfObservableVariable = new ObservableState<Tr>(initialValue);
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableVariable,
                resultObservable: Observable.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read())
            );
        }

        public static HalfDependencyState<Tr> Create<T1, T2, T3, Tr>(Tr initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, Func<Tr, T1, T2, T3, Tr> resultSelector)
        {
            var selfObservableVariable = new ObservableState<Tr>(initialValue);
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableVariable,
                resultObservable: Observable.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(), source3.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read(), source3.Read())
            );
        }

        public static HalfDependencyState<Tr> Create<T1, T2, T3, T4, Tr>(Tr initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, Func<Tr, T1, T2, T3, T4, Tr> resultSelector)
        {
            var selfObservableVariable = new ObservableState<Tr>(initialValue);
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableVariable,
                resultObservable: Observable.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read(), source3.Read(), source4.Read())
            );
        }

        public static HalfDependencyState<Tr> Create<T1, T2, T3, T4, T5, Tr>(Tr initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, Func<Tr, T1, T2, T3, T4, T5, Tr> resultSelector)
        {
            var selfObservableVariable = new ObservableState<Tr>(initialValue);
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableVariable,
                resultObservable: Observable.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read())
            );
        }

        public static HalfDependencyState<Tr> Create<T1, T2, T3, T4, T5, T6, Tr>(Tr initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, IStateObserver<T6> source6, Func<Tr, T1, T2, T3, T4, T5, T6, Tr> resultSelector)
        {
            var selfObservableVariable = new ObservableState<Tr>(initialValue);
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableVariable,
                resultObservable: Observable.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read())
            );
        }

        public static HalfDependencyState<Tr> Create<T1, T2, T3, T4, T5, T6, T7, Tr>(Tr initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, IStateObserver<T6> source6, IStateObserver<T7> source7, Func<Tr, T1, T2, T3, T4, T5, T6, T7, Tr> resultSelector)
        {
            var selfObservableVariable = new ObservableState<Tr>(initialValue);
            return new HalfDependencyState<Tr>(
                selfObservableState: selfObservableVariable,
                resultObservable: Observable.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(), source7.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read(), source7.Read())
            );
        }
    }
}