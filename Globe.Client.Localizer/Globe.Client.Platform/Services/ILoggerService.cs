using System;

namespace Globe.Client.Platform.Services
{
    public enum LogSeverity
    {
        Information = 0,
        Warning = 1,
        Error = 2,
        Fatal = 3
    }

    public interface ILoggerService
    {
        void Log(string message, LogSeverity severity);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Exception(Exception exception);
    }
}
