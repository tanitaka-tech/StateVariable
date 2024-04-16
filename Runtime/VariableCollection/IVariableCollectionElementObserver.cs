using R3;

namespace TanitakaTech.StateVariable.VariableCollection
{
    public interface IVariableCollectionElementObserver<T, ID> :
        IVariableCollectionElementReader<T, ID>
    {
        Observable<T> ObserveElement(ID id);
    }
}