using System;
using R3;

namespace TanitakaTech.StateVariable.R3Extensions
{
    public static class StateVariableR3Extensions
    {
        public static IVariableObserver<T> ToVariableObserver<T>(this Observable<T> observable, out IDisposable disposable)
        {
            var bridge = new VariableObserverBridge<T>(observable, out disposable);
            return bridge;
        }

        private class VariableObserverBridge<T> : 
            IVariableObserver<T>
        {
            private Observable<T> Observable { get; }
            private T _currentValue;
        
            public VariableObserverBridge(Observable<T> observable, out IDisposable disposable)
            {
                Observable = observable;
                disposable = Observable
                    .Subscribe(value =>
                    {
                        _currentValue = value;
                    });
            }

            T IVariableReader<T>.Read() => _currentValue;
            Observable<T> IVariableObserver<T>.Observe() => Observable;
        }
    }
}