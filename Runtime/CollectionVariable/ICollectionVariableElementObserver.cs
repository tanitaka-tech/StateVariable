using R3;

namespace TanitakaTech.StateVariable.CollectionVariable
{
    public interface ICollectionVariableElementObserver<T, ID> :
        ICollectionVariableElementReader<T, ID>
    {
        Observable<T> ObserveElement(ID id);
    }
}