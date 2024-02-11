using System;

namespace TanitakaTech.StateVariable
{
    public interface IVariableObserver<out T> : IVariableReader<T>
    {
        IObservable<T> Observe();
    }
}