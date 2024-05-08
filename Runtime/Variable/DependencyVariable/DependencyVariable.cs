using System;
using R3;

namespace TanitakaTech.StateVariable
{
    public class DependencyVariable<T> : IVariableObserver<T>
    {
        public T Read() => ResultSelector();
        public Observable<T> Observe() => ResultObservable;
        
        private Observable<T> ResultObservable { get; }
        private Func<T> ResultSelector { get; }
        
        public DependencyVariable(Observable<T> resultObservable, Func<T> resultSelector)
        {
            ResultSelector = resultSelector;
            ResultObservable = resultObservable;
        }
        
        public static DependencyVariable<TR> Create<T1, TR>(IVariableObserver<T1> source1, Func<T1, TR> resultSelector)
        {
            return new DependencyVariable<TR>(
                resultObservable: source1.Observe().Select(resultSelector),
                resultSelector: () => resultSelector(source1.Read())
            );
        }
        
        public static DependencyVariable<TR> Create<T1, T2, TR>(IVariableObserver<T1> source1, IVariableObserver<T2> source2, Func<T1, T2, TR> resultSelector)
        {
            return new DependencyVariable<TR>(
                resultObservable: ObservableExtensions.CombineLatest(
                    source1.Observe(), source2.Observe(),
                    resultSelector
                ),
                resultSelector: () => resultSelector(source1.Read(), source2.Read())
            );
        }
        
        public static DependencyVariable<TR> Create<T1, T2, T3, TR>(IVariableObserver<T1> source1, IVariableObserver<T2> source2, IVariableObserver<T3> source3, Func<T1, T2, T3, TR> resultSelector)
        {
            return new DependencyVariable<TR>(
                resultObservable: ObservableExtensions.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), 
                    resultSelector
                    ),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read())
                );
        }
        
        public static DependencyVariable<TR> Create<T1, T2, T3, T4, TR>(IVariableObserver<T1> source1, IVariableObserver<T2> source2, IVariableObserver<T3> source3, IVariableObserver<T4> source4, Func<T1, T2, T3, T4, TR> resultSelector)
        {
            return new DependencyVariable<TR>(
                resultObservable: ObservableExtensions.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(),
                    resultSelector
                ),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read())
            );
        }
        
        public static DependencyVariable<TR> Create<T1, T2, T3, T4, T5, TR>(IVariableObserver<T1> source1, IVariableObserver<T2> source2, IVariableObserver<T3> source3, IVariableObserver<T4> source4, IVariableObserver<T5> source5, Func<T1, T2, T3, T4, T5, TR> resultSelector)
        {
            return new DependencyVariable<TR>(
                resultObservable: ObservableExtensions.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(),
                    resultSelector
                ),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read())
            );
        }
        
        public static DependencyVariable<TR> Create<T1, T2, T3, T4, T5, T6, TR>(IVariableObserver<T1> source1, IVariableObserver<T2> source2, IVariableObserver<T3> source3, IVariableObserver<T4> source4, IVariableObserver<T5> source5, IVariableObserver<T6> source6, Func<T1, T2, T3, T4, T5, T6, TR> resultSelector)
        {
            return new DependencyVariable<TR>(
                resultObservable: ObservableExtensions.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(),
                    resultSelector
                ),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read())
            );
        }
        
        public static DependencyVariable<TR> Create<T1, T2, T3, T4, T5, T6, T7, TR>(IVariableObserver<T1> source1, IVariableObserver<T2> source2, IVariableObserver<T3> source3, IVariableObserver<T4> source4, IVariableObserver<T5> source5, IVariableObserver<T6> source6, IVariableObserver<T7> source7, Func<T1, T2, T3, T4, T5, T6, T7, TR> resultSelector)
        {
            return new DependencyVariable<TR>(
                resultObservable: ObservableExtensions.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(), source7.Observe(),
                    resultSelector
                ),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read(), source7.Read())
            );
        }
        
        public static DependencyVariable<TR> Create<T1, T2, T3, T4, T5, T6, T7, T8, TR>(IVariableObserver<T1> source1, IVariableObserver<T2> source2, IVariableObserver<T3> source3, IVariableObserver<T4> source4, IVariableObserver<T5> source5, IVariableObserver<T6> source6, IVariableObserver<T7> source7, IVariableObserver<T8> source8, Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> resultSelector)
        {
            return new DependencyVariable<TR>(
                resultObservable: ObservableExtensions.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(), source7.Observe(), source8.Observe(),
                    resultSelector
                ),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read(), source7.Read(), source8.Read())
            );
        }
        
        public static DependencyVariable<TR> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(IVariableObserver<T1> source1, IVariableObserver<T2> source2, IVariableObserver<T3> source3, IVariableObserver<T4> source4, IVariableObserver<T5> source5, IVariableObserver<T6> source6, IVariableObserver<T7> source7, IVariableObserver<T8> source8, IVariableObserver<T9> source9, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> resultSelector)
        {
            return new DependencyVariable<TR>(
                resultObservable: ObservableExtensions.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(), source7.Observe(), source8.Observe(), source9.Observe(),
                    resultSelector
                ),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read(), source7.Read(), source8.Read(), source9.Read())
            );
        }

        public static DependencyVariable<TR> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            IVariableObserver<T1> source1, IVariableObserver<T2> source2, IVariableObserver<T3> source3,
            IVariableObserver<T4> source4, IVariableObserver<T5> source5, IVariableObserver<T6> source6,
            IVariableObserver<T7> source7, IVariableObserver<T8> source8, IVariableObserver<T9> source9,
            IVariableObserver<T10> source10, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> resultSelector)
        {
            return new DependencyVariable<TR>(
                resultObservable: ObservableExtensions.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(),
                    source6.Observe(), source7.Observe(), source8.Observe(), source9.Observe(), source10.Observe(),
                    resultSelector
                ),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(),
                    source5.Read(), source6.Read(), source7.Read(), source8.Read(), source9.Read(), source10.Read())
            );
        }
    }
}