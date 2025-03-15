using System;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sulfur.Contract.Helpers;

namespace Sulfur.Driver
{
    public class DriverHttpClient
    {
        public ILogger Logger { get; }
        private readonly Uri _uri;

        public DriverHttpClient(string host, uint port, ILogger logger)
        {
            Logger = logger;
            _uri = new Uri($"{host}:{port}");
        }

        private async Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TRequest : class
            where TResponse : class
        {
            var contractAttribute = typeof(TRequest).GetCustomAttribute<MessageContractAttribute>();
            if (contractAttribute == null)
            {
                throw new InvalidOperationException($"Request type {typeof(TRequest).Name} is missing MessageContractAttribute.");
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string jsonData = JsonConvert.SerializeObject(request);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(_uri, content);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    Logger.Debug($"Response: {responseBody}");

                    return JsonConvert.DeserializeObject<TResponse>(responseBody);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public TResponse SendRequest<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            var cancellationTokenSource = new CancellationTokenSource();
            return SendRequestAsync<TRequest, TResponse>(request, cancellationTokenSource.Token).GetAwaiter().GetResult();
        }
    }
}