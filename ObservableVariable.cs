using System;
using UniRx;

namespace TanitakaTech.StateVariable
{
    public class ObservableVariable<T> : 
        IVariable<T>,
        IVariableObserver<T>,
        IDisposable
        where T : IEquatable<T>
    {
        private IVariable<T> Variable { get; }
        private readonly Subject<T> _onVariableChangedSubject;

        public ObservableVariable(T initialValue)
        {
            Variable = new Variable<T>(initialValue);
            _onVariableChangedSubject = new();
        }
        
        void IVariableSetter<T>.Set(T value)
        {
            bool isUpdate = !value.Equals(Variable.Read());
            if (isUpdate == false)
            {
                return;
            }
            
            Variable.Set(value);
            _onVariableChangedSubject.OnNext(value);
        }

        T IVariableReader<T>.Read() => Variable.Read();
        
        IObservable<T> IVariableObserver<T>.Observe() => _onVariableChangedSubject.StartWith(Variable.Read());

        void IDisposable.Dispose()
        {
            _onVariableChangedSubject.Dispose();
        }
    }
}