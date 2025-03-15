using System;
using System.Collections.Generic;
using System.Reflection;
using Sulfur.Contract.Communication;
using Sulfur.Contract.Helpers.Exts;

namespace Sulfur.Contract.Helpers
{
    public class Contracts
    {
        private Dictionary<string, IMessageHandler> _messageHandlers = new Dictionary<string, IMessageHandler>();
        private Dictionary<string, (Type reqType, Type resType)> _messageContracts = new Dictionary<string, (Type reqType, Type resType)>();

        public IReadOnlyDictionary<string, IMessageHandler> MessageHandlers => _messageHandlers;
        public IReadOnlyDictionary<string, (Type reqType, Type resType)> MessageContracts => _messageContracts;

        public void Scan()
        {
            var assembly = Assembly.GetExecutingAssembly();

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass)
                {
                    if (type.TryGetCustomAttribute(out MessageHandlerAttribute messageHandler))
                    {
                        RegisterHandler(messageHandler);
                    }

                    if (type.TryGetCustomAttribute(out MessageContractAttribute messageContractAttribute))
                    {
                        _messageContracts.TryAdd(messageContractAttribute.RequestUrl, (type, messageContractAttribute.ResponseType));
                    }
                }
            }
        }

        private void RegisterHandler(MessageHandlerAttribute messageHandler)
        {
            var handlerType = messageHandler.GetType();
            var contractAttribute = handlerType.GetCustomAttribute<MessageHandlerAttribute>();
            var requestType = contractAttribute?.RequestType;

            // Ensure request type is valid
            if (requestType == null)
            {
                Console.WriteLine($"Error: {handlerType.Name} is missing a valid request type.");
                return;
            }

            // Get the MessageContractAttribute from the request class
            var requestAttribute = requestType.GetCustomAttribute<MessageContractAttribute>();
            if (requestAttribute == null)
            {
                Console.WriteLine($"Error: {requestType.Name} does not have a MessageContractAttribute.");
                return;
            }

            var expectedResponseType = requestAttribute.ResponseType;

            // Verify generic types in handler
            var handlerInterface = handlerType.BaseType;
            if (handlerInterface == null || !handlerInterface.IsGenericType)
            {
                Console.WriteLine($"Error: {handlerType.Name} is not a valid generic handler.");
                return;
            }

            var handlerGenericArgs = handlerInterface.GetGenericArguments();
            if (handlerGenericArgs.Length != 2)
            {
                Console.WriteLine($"Error: {handlerType.Name} does not follow the expected generic pattern.");
                return;
            }

            var handlerRequestType = handlerGenericArgs[0];
            var handlerResponseType = handlerGenericArgs[1];

            if (handlerResponseType != expectedResponseType)
            {
                Console.WriteLine($"Error: {handlerType.Name} expects response type '{handlerResponseType.Name}' but '{requestType.Name}' defines '{expectedResponseType.Name}' as its response.");
                return;
            }

            // Instantiate the handler and register it
            if (Activator.CreateInstance(handlerType) is IMessageHandler handlerInstance)
            {
                _messageHandlers[requestAttribute.RequestUrl] = handlerInstance;
                Console.WriteLine($"Registered: {handlerType.Name} -> {requestAttribute.RequestUrl}");
            }
            else
            {
                Console.WriteLine($"Error: Failed to instantiate handler {handlerType.Name}.");
            }
        }
    }
}