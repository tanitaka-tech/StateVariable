namespace TanitakaTech.StateVariable
{
    public static class Request
    {
        public static RequestClasses<TRequest> CreateRequestPusherAndConsumer<TRequest>() where TRequest : class
        {
            var requestVariable = new Variable<TRequest>(null);
            var requestPusher = new RequestPusher<TRequest>(requestVariable);
            var requestConsumer = new RequestConsumer<TRequest>(requestVariable);
            return new RequestClasses<TRequest>(requestPusher, requestConsumer);
        }
    }
    
    public struct RequestClasses<TRequest> where TRequest : class
    {
        public RequestPusher<TRequest> Pusher { get; }
        public RequestConsumer<TRequest> Consumer { get; }

        public RequestClasses(RequestPusher<TRequest> pusher, RequestConsumer<TRequest> consumer)
        {
            Pusher = pusher;
            Consumer = consumer;
        }
    }
}