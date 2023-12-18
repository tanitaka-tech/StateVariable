namespace TanitakaTech.StateVariable
{
    public interface IVariableSetter<in T>
    {
        public void SetVariable(T value);
    }
}