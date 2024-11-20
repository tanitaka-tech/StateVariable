using System;
using R3;

namespace TanitakaTech.StateVariable
{
    public class SubjectState<T> :
        IStateSetter<T>,
        IStateObserver<T>,
        IDisposable
    {
        private Subject<T> Subject { get; }
        private T _beforeValue;

        public SubjectState(T initialValue)
        {
            Subject = new Subject<T>();
            Set(initialValue);
        }

        public void Set(T value)
        {
            Subject.OnNext(value);
            _beforeValue = value;
        }

        public T Read() => _beforeValue;

        public Observable<T> Observe() => Subject;

        public void Dispose()
        {
            Subject.Dispose();
        }
    }
}