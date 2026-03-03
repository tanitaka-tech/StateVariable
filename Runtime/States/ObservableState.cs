using System;
using R3;

namespace TanitakaTech.StateVariable
{
    [Serializable]
    public class ObservableState<T> : 
        IStateSetter<T>,
        IStateObserver<T>,
        IDisposable
    {
        private ReactiveProperty<T> _reactiveProperty;

        public ObservableState(T initialValue)
        {
            _reactiveProperty = new(initialValue);
        }

        public void Set(T value)
        {
            if (_reactiveProperty == null) return;
            _reactiveProperty.Value = value;
        }

        public T Read()
        {
            if (_reactiveProperty == null) return default;
            return _reactiveProperty.Value;
        }
        
        public Observable<T> Observe() => _reactiveProperty;

        public void Dispose()
        {
            _reactiveProperty?.Dispose();
            _reactiveProperty = null;
        }
    }
}