using ObservableCollections;
using R3;

namespace TanitakaTech.StateVariable.CollectionVariable
{
    public interface ICollectionVariableObserver<T>
        : ICollectionVariableReader<T>
    {
        Observable<CollectionAddEvent<T>> ObserveAdd();
        Observable<CollectionRemoveEvent<T>> ObserveRemove();
        Observable<CollectionReplaceEvent<T>> ObserveReplace();
        Observable<CollectionMoveEvent<T>> ObserveMove();
        Observable<Unit> ObserveReset();
        Observable<int> ObserveCountChanged();
        
        Observable<Unit> ObserveAllChanges()
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
                .ThrottleFirstFrame(1);
        }
    }
}