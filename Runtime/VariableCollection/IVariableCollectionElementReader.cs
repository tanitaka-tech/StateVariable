namespace TanitakaTech.StateVariable.VariableCollection
{
    public interface IVariableCollectionElementReader<T, ID> :
        IVariableCollectionReader<T>
    {
        T ReadElement(ID id);
    }
}