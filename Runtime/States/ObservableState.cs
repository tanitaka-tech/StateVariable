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
        private T _value;
        private Subject<T> _subject;

        public ObservableState(T initialValue)
        {
            _value = initialValue;
            _subject = new Subject<T>();
        }
        
        public void Set(T value)
        {
            bool isUpdate = !EqualityComparer<T>.Default.Equals(value, _value);
            _value = value;
            if (isUpdate)
            {
                _subject.OnNext(_value);
            }
        }

        internal void SetWithoutNotify(T value)
        {
            _value = value;
        }

        public T Read() => _value;
        
        public Observable<T> Observe() => _subject.Prepend(_value);

        public void Dispose()
        {
            _subject.Dispose();
        }
    }
}