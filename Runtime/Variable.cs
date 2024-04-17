namespace TanitakaTech.StateVariable
{
    public class Variable<T> :
        IVariableSetter<T>
    {
        private T _variable;

        public Variable(T initialValue)
        {
            _variable = initialValue;
        }
        
        public void Set(T value) => _variable = value;
        public T Read() => _variable;
    }
}