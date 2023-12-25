using System.Threading;
using Cysharp.Threading.Tasks;

namespace TanitakaTech.StateVariable
{
    public class RequestConsumer<TRequest> where TRequest : class
    {
        private IVariable<TRequest> RequestVariable { get; }

        public RequestConsumer(IVariable<TRequest> requestVariable)
        {
            RequestVariable = requestVariable;
        }

        public async UniTask<TRequest> WaitRequestAndConsumeAsync(CancellationToken cancellationToken)
        {
            await UniTask.WaitUntil(() => RequestVariable.Read() != null, cancellationToken: cancellationToken);
            var requestedValue = RequestVariable.Read();
            RequestVariable.Set(null);
            return requestedValue;
        }
    }
}