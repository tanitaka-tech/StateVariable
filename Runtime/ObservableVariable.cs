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
        private ReactiveProperty<T> ReactiveProperty { get; }

        public ObservableVariable(T initialValue)
        {
            ReactiveProperty = new(initialValue);
        }
        
        public void Set(T value)
        {
            ReactiveProperty.Value = value;
        }

        public T Read() => ReactiveProperty.Value;
        
        public IObservable<T> Observe() => ReactiveProperty;

        public void Dispose()
        {
            ReactiveProperty.Dispose();
        }
    }
}