using System;
using R3;

namespace TanitakaTech.StateVariable
{
    public class HalfDependencyVariable<T> : IVariableObserver<T>, IVariableSetter<T>, IDisposable
    {
        public void Set(T value) => SelfObservableVariable.Set(value);
        public T Read() => ResultSelector();
        public Observable<T> Observe() => ResultObservable;

        private ObservableVariable<T> SelfObservableVariable { get; }
        private Observable<T> ResultObservable { get; }
        private Func<T> ResultSelector { get; }

        public HalfDependencyVariable(ObservableVariable<T> selfObservableVariable, Observable<T> resultObservable,
            Func<T> resultSelector)
        {
            SelfObservableVariable = selfObservableVariable;
            ResultSelector = resultSelector;
            ResultObservable = resultObservable;
        }

        public void Dispose()
        {
            SelfObservableVariable.Dispose();
        }

        public static HalfDependencyVariable<TR> Create<T1, TR>(TR initialValue, IVariableObserver<T1> source1, Func<TR, T1, TR> resultSelector)
        {
            var selfObservableVariable = new ObservableVariable<TR>(initialValue);
            return new HalfDependencyVariable<TR>(
                selfObservableVariable: selfObservableVariable,
                resultObservable: ObservableExtensions.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read())
            );
        }

        public static HalfDependencyVariable<TR> Create<T1, T2, TR>(TR initialValue, IVariableObserver<T1> source1, IVariableObserver<T2> source2, Func<TR, T1, T2, TR> resultSelector)
        {
            var selfObservableVariable = new ObservableVariable<TR>(initialValue);
            return new HalfDependencyVariable<TR>(
                selfObservableVariable: selfObservableVariable,
                resultObservable: ObservableExtensions.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read())
            );
        }
        
        public static HalfDependencyVariable<TR> Create<T1, T2, T3, TR>(TR initialValue, IVariableObserver<T1> source1, IVariableObserver<T2> source2, IVariableObserver<T3> source3, Func<TR, T1, T2, T3, TR> resultSelector)
        {
            var selfObservableVariable = new ObservableVariable<TR>(initialValue);
            return new HalfDependencyVariable<TR>(
                selfObservableVariable: selfObservableVariable,
                resultObservable: ObservableExtensions.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(), source3.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read(), source3.Read())
            );
        }
        
        public static HalfDependencyVariable<TR> Create<T1, T2, T3, T4, TR>(TR initialValue, IVariableObserver<T1> source1, IVariableObserver<T2> source2, IVariableObserver<T3> source3, IVariableObserver<T4> source4, Func<TR, T1, T2, T3, T4, TR> resultSelector)
        {
            var selfObservableVariable = new ObservableVariable<TR>(initialValue);
            return new HalfDependencyVariable<TR>(
                selfObservableVariable: selfObservableVariable,
                resultObservable: ObservableExtensions.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read(), source3.Read(), source4.Read())
            );
        }
        
        public static HalfDependencyVariable<TR> Create<T1, T2, T3, T4, T5, TR>(TR initialValue, IVariableObserver<T1> source1, IVariableObserver<T2> source2, IVariableObserver<T3> source3, IVariableObserver<T4> source4, IVariableObserver<T5> source5, Func<TR, T1, T2, T3, T4, T5, TR> resultSelector)
        {
            var selfObservableVariable = new ObservableVariable<TR>(initialValue);
            return new HalfDependencyVariable<TR>(
                selfObservableVariable: selfObservableVariable,
                resultObservable: ObservableExtensions.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read())
            );
        }
        
        public static HalfDependencyVariable<TR> Create<T1, T2, T3, T4, T5, T6, TR>(TR initialValue, IVariableObserver<T1> source1, IVariableObserver<T2> source2, IVariableObserver<T3> source3, IVariableObserver<T4> source4, IVariableObserver<T5> source5, IVariableObserver<T6> source6, Func<TR, T1, T2, T3, T4, T5, T6, TR> resultSelector)
        {
            var selfObservableVariable = new ObservableVariable<TR>(initialValue);
            return new HalfDependencyVariable<TR>(
                selfObservableVariable: selfObservableVariable,
                resultObservable: ObservableExtensions.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read())
            );
        }
        
        public static HalfDependencyVariable<TR> Create<T1, T2, T3, T4, T5, T6, T7, TR>(TR initialValue, IVariableObserver<T1> source1, IVariableObserver<T2> source2, IVariableObserver<T3> source3, IVariableObserver<T4> source4, IVariableObserver<T5> source5, IVariableObserver<T6> source6, IVariableObserver<T7> source7, Func<TR, T1, T2, T3, T4, T5, T6, T7, TR> resultSelector)
        {
            var selfObservableVariable = new ObservableVariable<TR>(initialValue);
            return new HalfDependencyVariable<TR>(
                selfObservableVariable: selfObservableVariable,
                resultObservable: ObservableExtensions.CombineLatest(
                    selfObservableVariable.Observe(), source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(), source7.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(selfObservableVariable.Read(), source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read(), source7.Read())
            );
        }
    }
}