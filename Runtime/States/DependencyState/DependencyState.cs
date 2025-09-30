using System;
using R3;

namespace TanitakaTech.StateVariable
{
    public class DependencyState<T> : IStateObserver<T>
    {
        public T Read() => _resultSelector();
        public Observable<T> Observe() => _resultObservable;

        private readonly Observable<T> _resultObservable;
        private readonly Func<T> _resultSelector;

        public DependencyState(Observable<T> resultObservable, Func<T> resultSelector)
        {
            _resultSelector = resultSelector;
            _resultObservable = resultObservable;
        }

        public static DependencyState<Tr> Create<T1, Tr>(Observable<T1> source1, Func<T1, Tr> resultSelector)
        {
            var source1BeforeValue = default(T1);
            return new DependencyState<Tr>(
                resultObservable: source1
                    .Do(value => source1BeforeValue = value)
                    .Select(resultSelector)
                    .DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1BeforeValue)
            );
        }

        public static DependencyState<Tr> Create<T1, T2, Tr>(Observable<T1> source1, Observable<T2> source2, Func<T1, T2, Tr> resultSelector)
        {
            var source1BeforeValue = default(T1);
            var source2BeforeValue = default(T2);
            return new DependencyState<Tr>(
                resultObservable: Observable.CombineLatest(
                    source1.Do(value => source1BeforeValue = value),
                    source2.Do(value => source2BeforeValue = value),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1BeforeValue, source2BeforeValue)
            );
        }

        public static DependencyState<Tr> Create<T1, T2, T3, Tr>(Observable<T1> source1, Observable<T2> source2, Observable<T3> source3, Func<T1, T2, T3, Tr> resultSelector)
        {
            var source1BeforeValue = default(T1);
            var source2BeforeValue = default(T2);
            var source3BeforeValue = default(T3);
            return new DependencyState<Tr>(
                resultObservable: Observable.CombineLatest(
                    source1.Do(value => source1BeforeValue = value),
                    source2.Do(value => source2BeforeValue = value),
                    source3.Do(value => source3BeforeValue = value),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1BeforeValue, source2BeforeValue, source3BeforeValue)
            );
        }

        public static DependencyState<Tr> Create<T1, Tr>(IStateObserver<T1> source1, Func<T1, Tr> resultSelector)
        {
            return new DependencyState<Tr>(
                resultObservable: source1.Observe().Select(resultSelector).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read())
            );
        }

        public static DependencyState<Tr> Create<T1, T2, Tr>(IStateObserver<T1> source1, IStateObserver<T2> source2, Func<T1, T2, Tr> resultSelector)
        {
            return new DependencyState<Tr>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read())
            );
        }

        public static DependencyState<Tr> Create<T1, T2, T3, Tr>(IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, Func<T1, T2, T3, Tr> resultSelector)
        {
            return new DependencyState<Tr>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(),
                    resultSelector
                    ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read())
                );
        }

        public static DependencyState<Tr> Create<T1, T2, T3, T4, Tr>(IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, Func<T1, T2, T3, T4, Tr> resultSelector)
        {
            return new DependencyState<Tr>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read())
            );
        }

        public static DependencyState<Tr> Create<T1, T2, T3, T4, T5, Tr>(IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, Func<T1, T2, T3, T4, T5, Tr> resultSelector)
        {
            return new DependencyState<Tr>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read())
            );
        }

        public static DependencyState<Tr> Create<T1, T2, T3, T4, T5, T6, Tr>(IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, IStateObserver<T6> source6, Func<T1, T2, T3, T4, T5, T6, Tr> resultSelector)
        {
            return new DependencyState<Tr>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read())
            );
        }

        public static DependencyState<Tr> Create<T1, T2, T3, T4, T5, T6, T7, Tr>(IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, IStateObserver<T6> source6, IStateObserver<T7> source7, Func<T1, T2, T3, T4, T5, T6, T7, Tr> resultSelector)
        {
            return new DependencyState<Tr>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(), source7.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read(), source7.Read())
            );
        }

        public static DependencyState<Tr> Create<T1, T2, T3, T4, T5, T6, T7, T8, Tr>(IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, IStateObserver<T6> source6, IStateObserver<T7> source7, IStateObserver<T8> source8, Func<T1, T2, T3, T4, T5, T6, T7, T8, Tr> resultSelector)
        {
            return new DependencyState<Tr>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(), source7.Observe(), source8.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read(), source7.Read(), source8.Read())
            );
        }

        public static DependencyState<Tr> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, Tr>(IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3, IStateObserver<T4> source4, IStateObserver<T5> source5, IStateObserver<T6> source6, IStateObserver<T7> source7, IStateObserver<T8> source8, IStateObserver<T9> source9, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Tr> resultSelector)
        {
            return new DependencyState<Tr>(
                resultObservable: Observable.CombineLatest(
                    source1.Observe(), source2.Observe(), source3.Observe(), source4.Observe(), source5.Observe(), source6.Observe(), source7.Observe(), source8.Observe(), source9.Observe(),
                    resultSelector
                ).DistinctUntilChanged(),
                resultSelector: () => resultSelector(source1.Read(), source2.Read(), source3.Read(), source4.Read(), source5.Read(), source6.Read(), source7.Read(), source8.Read(), source9.Read())
            );
        }

        public static DependencyState<Tr> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Tr>(
            IStateObserver<T1> source1, IStateObserver<T2> source2, IStateObserver<T3> source3,
            IStateObserver<T4> source4, IStateObserver<T5> source5, IStateObserver<T6> source6,
            IStateObserver<T7> source7, IStateObserver<T8> source8, IStateObserver<T9> source9,
            IStateObserver<T10> source10, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Tr> resultSelector)
        {
            return new DependencyState<Tr>(
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