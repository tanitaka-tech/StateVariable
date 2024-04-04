using R3;

namespace TanitakaTech.StateVariable
{
    public interface IVariableObserver<T> : IVariableReader<T>
    {
        Observable<T> Observe();
    }
}