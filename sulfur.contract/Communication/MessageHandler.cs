using Newtonsoft.Json;
using Sulfur.Contract.DataModels.Xml;
using Sulfur.Contract.Helpers;

namespace Sulfur.Contract.Communication
{
    public interface IMessageHandler
    {
        string OnMessageReceived(string requestMessage, ILogger logger, IXmlDom xmlDom);
    }

    public abstract class MessageHandler<TRequest, TResponse> : IMessageHandler where TRequest : RequestMessage where TResponse : ResponseMessage
    {
        public string OnMessageReceived(string requestMessage, ILogger logger, IXmlDom xmlDom)
        {
            logger.Debug($"[REQ] {requestMessage}");
            var requestInstance = JsonConvert.DeserializeObject<TRequest>(requestMessage);
            var response = HandleRequest(requestInstance, logger, xmlDom);
            var responseString = JsonConvert.SerializeObject(response);
            
            logger.Debug($"[RES] {responseString}");
            return responseString;
        }

        public abstract TResponse HandleRequest(TRequest request, ILogger logger, IXmlDom xmlDom);
    }
}