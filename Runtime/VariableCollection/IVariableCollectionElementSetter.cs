namespace TanitakaTech.StateVariable.VariableCollection
{
    public interface IVariableCollectionElementSetter<TKey, TValue> :
        IVariableCollectionElementReader<TKey, TValue>
    {
        void SetElement(TKey id, TValue newElement);
    }
}