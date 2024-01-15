namespace TanitakaTech.StateVariable
{
    public interface IVariableSetter<in T>
    {
        public void Set(T value);
    }
}