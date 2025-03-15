using Sulfur.Contract.Helpers;

namespace sulfur.unity_sdk
{
    public class UnityLogger: ILogger
    {
        public string Context { get; }

        public UnityLogger(string context)
        {
            Context = context;
        }
        
        public void Debug(string message) => UnityEngine.Debug.Log($"[{Context}] {message}");

        public void Warn(string message) => UnityEngine.Debug.LogWarning($"[{Context}] {message}");

        public void Error(string message) => UnityEngine.Debug.LogError($"[{Context}] {message}");
    }
}