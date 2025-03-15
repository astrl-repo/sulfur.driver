namespace Sulfur.Contract.Helpers
{
    public interface ILogger
    {
        void Debug(string message);
        void Warn(string message);
        void Error(string message);
    }
}