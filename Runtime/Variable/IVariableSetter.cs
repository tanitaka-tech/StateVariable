namespace TanitakaTech.StateVariable
{
    public interface IVariableSetter<T> : IVariableReader<T>
    {
        public void Set(T value);
    }
}