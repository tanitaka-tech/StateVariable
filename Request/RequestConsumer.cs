using System.Threading;
using Cysharp.Threading.Tasks;

namespace TanitakaTech.StateVariable
{
    public class RequestConsumer<TRequest> where TRequest : class
    {
        private Variable<TRequest> RequestVariable { get; }

        public RequestConsumer(Variable<TRequest> requestVariable)
        {
            RequestVariable = requestVariable;
        }

        public async UniTask<TRequest> WaitRequestAndConsumeAsync(CancellationToken cancellationToken)
        {
            await UniTask.WaitUntil(() => RequestVariable.CurrentVariable != null, cancellationToken: cancellationToken);
            var requestedValue = RequestVariable.CurrentVariable;
            RequestVariable.SetVariable(null);
            return requestedValue;
        }
    }
}