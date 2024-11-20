using System;
using R3;

namespace TanitakaTech.StateVariable
{
    public class HalfDependencyState<T> : IStateObserver<T>, IStateSetter<T>, IDisposable
    {
        public void Set(T value) => SelfObservableState.Set(value);
        public T Read() => ResultSelector();
        public Observable<T> Observe() => ResultObservable;

        private ObservableState<T> SelfObservableState { get; }
        private Observable<T> ResultObservable { get; }
        private Func<T> ResultSelector { get; }

        public HalfDependencyState(ObservableState<T> selfObservableState, Observable<T> resultObservable,
            Func<T> resultSelector)
        {
            SelfObservableState = selfObservableState;
            ResultSelector = resultSelector;
            ResultObservable = resultObservable;
        }

        public void Dispose()
        {
            SelfObservableState.Dispose();
        }
        
        public static HalfDependencyState<TR> Create<TR>(TR initialValue, Func<TR, TR> resultSelector)
        {
            var selfObservableVariable = new ObservableState<TR>(initialValue);
            return new HalfDependencyState<TR>(
                selfObservableState: selfObservableVariable,
                resultObservable: selfObservableVariable.Observe().Select(resultSelector).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read())
            );
        }

        public static HalfDependencyState<TR> Create<T1, TR>(TR initialValue, IStateObserver<T1> source1, Func<TR, T1, TR> resultSelector)
        {
            var selfObservableVariable = new ObservableState<TR>(initialValue);
            return new HalfDependencyState<TR>(
                selfObservableState: selfObservableVariable,
                resultObservable: Observable.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read())
            );
        }

        public static HalfDependencyState<TR> Create<T1, T2, TR>(TR initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, Func<TR, T1, T2, TR> resultSelector)
        {
            var selfObservableVariable = new ObservableState<TR>(initialValue);
            return new HalfDependencyState<TR>(
                selfObservableState: selfObservableVariable,
                resultObservable: Observable.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read())
            );
        }
        
        public static HalfDependencyState<TR> Create<T1, T2, T3, TR>(TR initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, Func<TR, T1, T2, T3, TR> resultSelector)
        {
            var selfObservableVariable = new ObservableState<TR>(initialValue);
            return new HalfDependencyState<TR>(
                selfObservableState: selfObservableVariable,
                resultObservable: Observable.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(), source3.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read(), source3.Read())
            );
        }
        
        public static HalfDependencyState<TR> Create<T1, T2, T3, T4, TR>(TR initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, Func<TR, T1, T2, T3, T4, TR> resultSelector)
        {
            var selfObservableVariable = new ObservableState<TR>(initialValue);
            return new HalfDependencyState<TR>(
                selfObservableState: selfObservableVariable,
                resultObservable: Observable.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read(), source3.Read(), source4.Read())
            );
        }
        
        public static HalfDependencyState<TR> Create<T1, T2, T3, T4, T5, TR>(TR initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, Func<TR, T1, T2, T3, T4, T5, TR> resultSelector)
        {
            var selfObservableVariable = new ObservableState<TR>(initialValue);
            return new HalfDependencyState<TR>(
                selfObservableState: selfObservableVariable,
                resultObservable: Observable.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read())
            );
        }
        
        public static HalfDependencyState<TR> Create<T1, T2, T3, T4, T5, T6, TR>(TR initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, IStateObserver<T6> source6, Func<TR, T1, T2, T3, T4, T5, T6, TR> resultSelector)
        {
            var selfObservableVariable = new ObservableState<TR>(initialValue);
            return new HalfDependencyState<TR>(
                selfObservableState: selfObservableVariable,
                resultObservable: Observable.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read())
            );
        }
        
        public static HalfDependencyState<TR> Create<T1, T2, T3, T4, T5, T6, T7, TR>(TR initialValue, IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, IStateObserver<T6> source6, IStateObserver<T7> source7, Func<TR, T1, T2, T3, T4, T5, T6, T7, TR> resultSelector)
        {
            var selfObservableVariable = new ObservableState<TR>(initialValue);
            return new HalfDependencyState<TR>(
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