namespace TanitakaTech.StateVariable
{
    public class State<T> :
        IStateSetter<T>
    {
        private T _value;

        public State(T initialValue)
        {
            _value = initialValue;
        }
        
        public void Set(T value) => _value = value;
        public T Read() => _value;
    }
}