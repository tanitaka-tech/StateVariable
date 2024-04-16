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
                .ThrottleLastFrame(1);
        }

        Observable<IEnumerable<T>> IVariableObserver<IEnumerable<T>>.Observe()
        {
            return ObserveAllChanges()
                .Select(_ => Read());
        }
    }
}