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

        Observable<Unit> ObserveAllChangesInternal();

        Observable<IEnumerable<T>> IStateObserver<IEnumerable<T>>.Observe()
        {
            return ObserveAllChangesInternal()
                .Select(_ => Read());
        }
    }
}