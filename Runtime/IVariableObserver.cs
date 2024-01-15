using System;

namespace TanitakaTech.StateVariable
{
    public interface IVariableObserver<out T>
    {
        IObservable<T> Observe();
    }
}