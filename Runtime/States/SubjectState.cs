using System;
using R3;

namespace TanitakaTech.StateVariable
{
    public class SubjectState<T> :
        IStateSetter<T>,
        IStateObserver<T>,
        IDisposable
    {
        private readonly Subject<T> _subject;
        private T _beforeValue;

        public SubjectState(T initialValue)
        {
            _subject = new Subject<T>();
            Set(initialValue);
        }

        public void Set(T value)
        {
            _subject.OnNext(value);
            _beforeValue = value;
        }

        public T Read() => _beforeValue;

        public Observable<T> Observe() => _subject.Prepend(_beforeValue);

        public void Dispose()
        {
            _subject.Dispose();
        }
    }
}
