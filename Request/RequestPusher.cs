using UnityEngine;

namespace TanitakaTech.StateVariable
{
    public class RequestPusher<TRequest> where TRequest : class
    {
        private IVariableSetter<TRequest> RequestSetter { get; }

        public RequestPusher(IVariableSetter<TRequest> requestSetter)
        {
            RequestSetter = requestSetter;
        }

        public void PushRequest(TRequest requestValue)
        {
            Debug.Log($"PushRequest: {requestValue}");
            RequestSetter.SetVariable(requestValue);
        }
    }
}