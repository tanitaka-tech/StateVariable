using System;
using UniRx;

namespace TanitakaTech.StateVariable
{
    public class ObservableVariable<T> : Variable<T>,
        IVariableObserver<T>,
        IDisposable
    {
        private readonly Subject<T> _onVariableChangedSubject;

        public ObservableVariable(T initialValue) : base(initialValue)
        {
            _onVariableChangedSubject = new();
        }
        
        public override void SetVariable(T value)
        {
            base.SetVariable(value);
            _onVariableChangedSubject.OnNext(value);
        }
        
        IObservable<T> IVariableObserver<T>.Observe() => _onVariableChangedSubject.StartWith(CurrentVariable);

        void IDisposable.Dispose()
        {
            _onVariableChangedSubject.Dispose();
        }
    }
}