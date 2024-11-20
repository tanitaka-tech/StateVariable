namespace TanitakaTech.StateVariable
{
    public class State<T> :
        IStateSetter<T>
    {
        private T _variable;

        public State(T initialValue)
        {
            _variable = initialValue;
        }
        
        public void Set(T value) => _variable = value;
        public T Read() => _variable;
    }
}