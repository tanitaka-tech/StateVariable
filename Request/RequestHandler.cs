using System.Threading;
using Cysharp.Threading.Tasks;

namespace TanitakaTech.StateVariable
{
    public class RequestHandler<TRequest> :
        IRequestPusher<TRequest>,
        IRequestConsumer<TRequest> where TRequest : class
    {
        private IVariable<TRequest> RequestVariable { get; } = new Variable<TRequest>(null);

        void IRequestPusher<TRequest>.PushRequest(TRequest requestValue)
        {
            RequestVariable.Set(requestValue);
        }

        async UniTask<TRequest> IRequestConsumer<TRequest>.WaitRequestAndConsumeAsync(CancellationToken cancellationToken)
        {
            await UniTask.WaitUntil(() => RequestVariable.Read() != null, cancellationToken: cancellationToken);
            var requestedValue = RequestVariable.Read();
            RequestVariable.Set(null);
            return requestedValue;
        }
    }
}