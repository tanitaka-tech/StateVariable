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
        [SerializeField] private SerializableReactiveProperty<T> _reactiveProperty;

        public ObservableVariable(T initialValue)
        {
            _reactiveProperty = new(initialValue);
        }
        
        public void Set(T value)
        {
            _reactiveProperty.Value = value;
        }

        public T Read() => _reactiveProperty.Value;
        
        public Observable<T> Observe() => _reactiveProperty;

        public void Dispose()
        {
            _reactiveProperty.Dispose();
        }
    }
}