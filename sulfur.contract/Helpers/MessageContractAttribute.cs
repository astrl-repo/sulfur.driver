using System;

namespace Sulfur.Contract.Helpers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageContractAttribute : Attribute
    {
        public MessageContractAttribute(string requestUrl, Type responseType)
        {
            RequestUrl = requestUrl;
            ResponseType = responseType;
        }

        public string RequestUrl { get; }
        public Type ResponseType { get; }
    }
}