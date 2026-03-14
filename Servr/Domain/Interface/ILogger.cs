namespace Servr.Domain.Interface
{
    public enum LogLevel
    {
        INFO,
        WARNING,
        ERROR,
    }

    public interface ILogger
    {
        void Log(LogLevel level, string message);
    }
}
