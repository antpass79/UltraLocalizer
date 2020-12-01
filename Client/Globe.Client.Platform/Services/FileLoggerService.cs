using Globe.Client.Platform.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Globe.Client.Platform.Services
{
    public class FileLoggerService : ILoggerService
    {
        string _path;

        public FileLoggerService()
        {
            var folder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Logs");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            _path = Path.Combine(folder, $"log-{Process.GetCurrentProcess().Id}.txt");
        }

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
            using (StreamWriter streamWriter = File.AppendText(_path))
            {
                var builder = new StringBuilder($"##### - {severity} - #####");
                builder.Append(Environment.NewLine);
                builder.Append(message);
                builder.Append(Environment.NewLine);

                streamWriter.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.ffff")}-{severity}: {builder.ToString()}");
            }
        }
    }
}
