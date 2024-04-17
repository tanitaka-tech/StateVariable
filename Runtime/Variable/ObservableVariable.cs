using System;
using R3;
using UnityEngine;

namespace TanitakaTech.StateVariable
{
    [Serializable]
    public class ObservableVariable<T> : 
        IVariableSetter<T>,
        IVariableObserver<T>,
        IDisposable
    {
        [SerializeField] private SerializableReactiveProperty<T> ReactiveProperty;

        public ObservableVariable(T initialValue)
        {
            ReactiveProperty = new(initialValue);
        }
        
        public void Set(T value)
        {
            ReactiveProperty.Value = value;
        }

        public T Read() => ReactiveProperty.Value;
        
        public Observable<T> Observe() => ReactiveProperty;

        public void Dispose()
        {
            ReactiveProperty.Dispose();
        }
    }
}