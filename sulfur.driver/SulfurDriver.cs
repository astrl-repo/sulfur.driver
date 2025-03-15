using System;
using System.Threading;
using Sulfur.Contract;
using Sulfur.Contract.Communication.Command;
using Sulfur.Contract.Helpers;

namespace Sulfur.Driver
{
    public class SulfurDriver: IDisposable
    {
        public ILogger Logger { get; }
        private DriverHttpClient _client;
        private readonly CancellationTokenSource _cts;

        public DriverHttpClient Client
        {
            get
            {
                _client ??= new DriverHttpClient($"ws://127.0.0.1", Const.DEFAULT_PORT, Logger);

                return _client;
            }
        }

        public SulfurDriver(ILogger logger)
        {
            Logger = logger;
            _cts = new CancellationTokenSource();
        }

        public SulfurObject Find(string xpath)
        {
            var response = Client.SendRequest<FindObjectRequest, FindObjectResponse>(new FindObjectRequest(xpath));
            return new SulfurObject(this, response.Data);
        }

        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
}