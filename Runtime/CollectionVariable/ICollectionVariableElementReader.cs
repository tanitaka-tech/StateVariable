namespace TanitakaTech.StateVariable.CollectionVariable
{
    public interface ICollectionVariableElementReader<T, ID> :
        ICollectionVariableReader<T>
    {
        T ReadElement(ID id);
    }
}