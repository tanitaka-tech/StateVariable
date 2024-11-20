using System;
using R3;

namespace TanitakaTech.StateVariable.R3Extensions
{
    public static class StateVariableR3Extensions
    {
        public static IStateObserver<T> ToStateObserver<T>(this Observable<T> observable, out IDisposable disposable)
        {
            var bridge = new StateObserverBridge<T>(observable, out disposable);
            return bridge;
        }

        private class StateObserverBridge<T> : 
            IStateObserver<T>
        {
            private Observable<T> Observable { get; }
            private T _currentValue;
        
            public StateObserverBridge(Observable<T> observable, out IDisposable disposable)
            {
                Observable = observable;
                disposable = Observable
                    .Subscribe(value =>
                    {
                        _currentValue = value;
                    });
            }

            T IStateReader<T>.Read() => _currentValue;
            Observable<T> IStateObserver<T>.Observe() => Observable;
        }
    }
}