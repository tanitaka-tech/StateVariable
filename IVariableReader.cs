namespace TanitakaTech.StateVariable
{
    public interface IVariableReader<out T>
    {
        T Read();
    }
}