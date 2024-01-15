namespace TanitakaTech.StateVariable
{
    public interface IRequestPusher<TRequest> where TRequest : class
    {
        public void PushRequest(TRequest requestValue);
    }
}