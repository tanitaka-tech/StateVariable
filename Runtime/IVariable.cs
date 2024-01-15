namespace TanitakaTech.StateVariable
{
    public interface IVariable<T> :
        IVariableReader<T>,
        IVariableSetter<T>
    {
    }
}