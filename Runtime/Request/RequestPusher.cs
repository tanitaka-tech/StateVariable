namespace TanitakaTech.StateVariable
{
    public class RequestPusher<TRequest> : IRequestPusher<TRequest>
        where TRequest : class
    {
        private IVariableSetter<TRequest> RequestSetter { get; }

        public RequestPusher(IVariableSetter<TRequest> requestSetter)
        {
            RequestSetter = requestSetter;
        }

        public void PushRequest(TRequest requestValue)
        {
            RequestSetter.Set(requestValue);
        }
    }
}