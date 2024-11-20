namespace TanitakaTech.StateVariable
{
    public interface IStateSetter<T> : IStateReader<T>
    {
        public void Set(T value);
    }
}