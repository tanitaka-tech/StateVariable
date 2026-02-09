using System.Collections.Generic;
using ObservableCollections;
using R3;

namespace TanitakaTech.StateVariable.StateCollections
{
    public interface IStateCollectionObserver<T> :
        IStateCollectionReader<T>,
        IStateObserver<IEnumerable<T>>
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

        Observable<IEnumerable<T>> IStateObserver<IEnumerable<T>>.Observe()
        {
            return ObserveAllChangesInternal()
                .Select(_ => Read());
        }
    }
}