namespace TanitakaTech.StateVariable
{
    public class Variable<T> :
        IVariable<T>
    {
        public T CurrentVariable => _variable;
        private T _variable;

        public Variable(T initialValue)
        {
            _variable = initialValue;
        }
        
        void IVariableSetter<T>.Set(T value) => _variable = value;
        T IVariableReader<T>.Read() => CurrentVariable;
    }
}