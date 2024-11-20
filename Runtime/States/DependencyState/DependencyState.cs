using System;
using R3;

namespace TanitakaTech.StateVariable
{
    public class DependencyState<T> : IStateObserver<T>
    {
        public T Read() => ResultSelector();
        public Observable<T> Observe() => ResultObservable;
        
        private Observable<T> ResultObservable { get; }
        private Func<T> ResultSelector { get; }
        
        public DependencyState(Observable<T> resultObservable, Func<T> resultSelector)
        {
            ResultSelector = resultSelector;
            ResultObservable = resultObservable;
        }
        
        public static DependencyState<TR> Create<T1, TR>(IStateObserver<T1> source1, Func<T1, TR> resultSelector)
        {
            return new DependencyState<TR>(
                resultObservable: source1.Observe().Select(resultSelector).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read())
            );
        }
        
        public static DependencyState<TR> Create<T1, T2, TR>(IStateObserver<T1> source1, IStateObserver<T2> source2, Func<T1, T2, TR> resultSelector)
        {
            return new DependencyState<TR>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read())
            );
        }
        
        public static DependencyState<TR> Create<T1, T2, T3, TR>(IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, Func<T1, T2, T3, TR> resultSelector)
        {
            return new DependencyState<TR>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), 
                    resultSelector
                    ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read())
                );
        }
        
        public static DependencyState<TR> Create<T1, T2, T3, T4, TR>(IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, Func<T1, T2, T3, T4, TR> resultSelector)
        {
            return new DependencyState<TR>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read())
            );
        }
        
        public static DependencyState<TR> Create<T1, T2, T3, T4, T5, TR>(IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, Func<T1, T2, T3, T4, T5, TR> resultSelector)
        {
            return new DependencyState<TR>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read())
            );
        }
        
        public static DependencyState<TR> Create<T1, T2, T3, T4, T5, T6, TR>(IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, IStateObserver<T6> source6, Func<T1, T2, T3, T4, T5, T6, TR> resultSelector)
        {
            return new DependencyState<TR>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read())
            );
        }
        
        public static DependencyState<TR> Create<T1, T2, T3, T4, T5, T6, T7, TR>(IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, IStateObserver<T6> source6, IStateObserver<T7> source7, Func<T1, T2, T3, T4, T5, T6, T7, TR> resultSelector)
        {
            return new DependencyState<TR>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(), source7.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read(), source7.Read())
            );
        }
        
        public static DependencyState<TR> Create<T1, T2, T3, T4, T5, T6, T7, T8, TR>(IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, IStateObserver<T6> source6, IStateObserver<T7> source7, IStateObserver<T8> source8, Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> resultSelector)
        {
            return new DependencyState<TR>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(), source7.Observe(), source8.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read(), source7.Read(), source8.Read())
            );
        }
        
        public static DependencyState<TR> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, IStateObserver<T6> source6, IStateObserver<T7> source7, IStateObserver<T8> source8, IStateObserver<T9> source9, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> resultSelector)
        {
            return new DependencyState<TR>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(), source7.Observe(), source8.Observe(), source9.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read(), source7.Read(), source8.Read(), source9.Read())
            );
        }

        public static DependencyState<TR> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3,
            IStateObserver<T4> source4, IStateObserver<T5> source5, IStateObserver<T6> source6,
            IStateObserver<T7> source7, IStateObserver<T8> source8, IStateObserver<T9> source9,
            IStateObserver<T10> source10, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> resultSelector)
        {
            return new DependencyState<TR>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(),
                    source6.Observe(), source7.Observe(), source8.Observe(), source9.Observe(), source10.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(),
                    source5.Read(), source6.Read(), source7.Read(), source8.Read(), source9.Read(), source10.Read())
            );
        }
    }
}