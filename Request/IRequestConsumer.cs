using System.Threading;
using Cysharp.Threading.Tasks;

namespace TanitakaTech.StateVariable
{
    public interface IRequestConsumer<TRequest> where TRequest : class
    {
        public UniTask<TRequest> WaitRequestAndConsumeAsync(CancellationToken cancellationToken);
    }
}