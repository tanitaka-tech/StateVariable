using ObservableCollections;
using R3;

namespace TanitakaTech.StateVariable.CollectionVariable
{
    public interface ICollectionVariableObserver<T>
    {
        Observable<CollectionAddEvent<T>> ObserveAdd();
        Observable<CollectionRemoveEvent<T>> ObserveRemove();
        Observable<CollectionReplaceEvent<T>> ObserveReplace();
        Observable<CollectionMoveEvent<T>> ObserveMove();
        Observable<Unit> ObserveReset();
        Observable<int> ObserveCountChanged();
    }
}