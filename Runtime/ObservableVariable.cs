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
        
        void IVariableSetter<T>.Set(T value)
        {
            ReactiveProperty.Value = value;
        }

        T IVariableReader<T>.Read() => ReactiveProperty.Value;
        
        IObservable<T> IVariableObserver<T>.Observe() => ReactiveProperty.StartWith(ReactiveProperty.Value);

        void IDisposable.Dispose()
        {
            ReactiveProperty.Dispose();
        }
    }
}