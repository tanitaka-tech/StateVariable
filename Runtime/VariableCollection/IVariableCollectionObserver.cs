using System.Collections.Generic;
using ObservableCollections;
using R3;

namespace TanitakaTech.StateVariable.VariableCollection
{
    public interface IVariableCollectionObserver<T> : 
        IVariableCollectionReader<T>,
        IVariableObserver<IEnumerable<T>>
    {
        Observable<CollectionAddEvent<T>> ObserveAdd();
        Observable<CollectionRemoveEvent<T>> ObserveRemove();
        Observable<CollectionReplaceEvent<T>> ObserveReplace();
        Observable<CollectionMoveEvent<T>> ObserveMove();
        Observable<Unit> ObserveReset();
        Observable<int> ObserveCountChanged();
        
        private Observable<Unit> ObserveAllChangesInternal()
        {
            return Observable
                .Merge(
                    ObserveAdd().AsUnitObservable()
                    , ObserveRemove().AsUnitObservable()
                    , ObserveReplace().AsUnitObservable()
                    , ObserveMove().AsUnitObservable()
                    , ObserveReset().AsUnitObservable()
                    , ObserveCountChanged().AsUnitObservable()
                )
                .Prepend(Unit.Default)
                .ThrottleLastFrame(1);
        }

        Observable<IEnumerable<T>> IVariableObserver<IEnumerable<T>>.Observe()
        {
            return ObserveAllChangesInternal()
                .Select(_ => Read());
        }

        public new Observable<IEnumerable<T>> Observe() => ((IVariableObserver<IEnumerable<T>>)this).Observe();
    }
}