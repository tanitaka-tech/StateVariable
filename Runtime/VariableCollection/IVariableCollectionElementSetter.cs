namespace TanitakaTech.StateVariable.VariableCollection
{
    public interface IVariableCollectionElementSetter<T, ID> :
        IVariableCollectionElementReader<T, ID>
    {
        void SetElement(ID id, T newElement);
    }
}