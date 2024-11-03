using System;
using R3;

namespace TanitakaTech.StateVariable
{
    public class SubjectVariable<T> :
        IVariableSetter<T>,
        IVariableObserver<T>,
        IDisposable
    {
        private Subject<T> Subject { get; }
        private T _beforeValue;

        public SubjectVariable(T initialValue)
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