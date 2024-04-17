using R3;

namespace TanitakaTech.StateVariable.VariableCollection
{
    public interface IVariableCollectionElementObserver<TKey, TValue> :
        IVariableCollectionElementReader<TKey, TValue>
    {
        Observable<TValue> ObserveElement(TKey id);
    }
}