using Globe.Client.Platform.Extensions;
using System;
using System.Text;

namespace Globe.Client.Platform.Services
{
    public class ConsoleLoggerService : ILoggerService
    {
        public void Error(string message)
        {
            InternalLog(message, LogSeverity.Error);
        }

        public void Exception(Exception exception)
        {
            InternalLog(exception.Dump(), LogSeverity.Error);
        }

        public void Info(string message)
        {
            InternalLog(message, LogSeverity.Information);
        }

        public void Log(string message, LogSeverity severity)
        {
            InternalLog(message, severity);
        }

        public void Warn(string message)
        {
            InternalLog(message, LogSeverity.Warning);
        }

        private void InternalLog(string message, LogSeverity severity)
        {
            var builder = new StringBuilder($"##### - {severity} - #####");
            builder.Append(Environment.NewLine);
            builder.Append(message);
            builder.Append(Environment.NewLine);

            Console.Write(builder.ToString());
        }
    }
}
