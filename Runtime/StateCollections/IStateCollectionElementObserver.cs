using R3;

namespace TanitakaTech.StateVariable.VariableCollection
{
    public interface IStateCollectionElementObserver<TKey, TValue> :
        IStateCollectionElementReader<TKey, TValue>
    {
        Observable<TValue> ObserveElement(TKey id);
    }
}