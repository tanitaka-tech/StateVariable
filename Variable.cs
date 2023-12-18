namespace TanitakaTech.StateVariable
{
    public class Variable<T> :
        IVariableReader<T>,
        IVariableSetter<T>
    {
        public T CurrentVariable => _variable;
        private T _variable;

        public Variable(T initialValue)
        {
            _variable = initialValue;
        }
        
        void IVariableSetter<T>.SetVariable(T value)=> SetVariable(value);
        public virtual void SetVariable(T value) => _variable = value;

        T IVariableReader<T>.Read() => CurrentVariable;
    }
}