using System;

namespace Sulfur.Contract.Helpers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageHandlerAttribute : Attribute
    {
        public Type RequestType { get; }

        public MessageHandlerAttribute(Type requestType)
        {
            RequestType = requestType;
        }
    }
}