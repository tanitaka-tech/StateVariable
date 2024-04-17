namespace TanitakaTech.StateVariable.VariableCollection
{
    public interface IVariableCollectionElementReader<TKey, TValue> :
        IVariableCollectionReader<TValue>
    {
        TValue ReadElement(TKey id);
    }
}