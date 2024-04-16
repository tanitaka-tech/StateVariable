namespace TanitakaTech.StateVariable.CollectionVariable
{
    public interface ICollectionVariableElementSetter<T, ID> :
        ICollectionVariableElementReader<T, ID>
    {
        void SetElement(ID id, T newElement);
    }
}