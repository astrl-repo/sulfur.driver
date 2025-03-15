
using System;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

namespace sulfur.unity_sdk.Connection
{
    public class HttpService
    {
        private HttpListener _listener;
        private Thread _serverThread;
        private uint _port;

        public HttpService(uint port)
        {
            _port = port;
        }

        public void StartServer()
        {
            _serverThread = new Thread(RunServer);
            _serverThread.IsBackground = true;
            _serverThread.Start();
        }

        public void StopServer()
        {
            _listener?.Stop();
            _serverThread?.Abort();
        }

        void RunServer()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://localhost:{_port}/");
            _listener.Start();
            Debug.Log($"HTTP Server started on port {_port}");

            while (_listener.IsListening)
            {
                try
                {
                    HttpListenerContext context = _listener.GetContext(); // Blocks until request
                    HttpListenerResponse response = context.Response;

                    string responseString = "<html><body><h1>Unity HTTP Server</h1></body></html>";
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                    response.OutputStream.Close();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Server error: {ex.Message}");
                }
            }
        }
    }
}