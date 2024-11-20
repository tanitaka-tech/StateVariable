using R3;

namespace TanitakaTech.StateVariable
{
    public interface IStateObserver<T> : IStateReader<T>
    {
        Observable<T> Observe();
    }
}