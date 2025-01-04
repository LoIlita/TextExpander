using System;
using System.Text;
using TextExpander.Interfaces;

namespace TextExpander.Services
{
    public class FileLogger : ILogger
    {
        private readonly string _logPath;
        private static readonly Encoding _encoding = Encoding.UTF8;

        public FileLogger(string logPath)
        {
            _logPath = logPath;
            // Upewnij się, że plik zostanie utworzony z odpowiednim kodowaniem
            if (!System.IO.File.Exists(logPath))
            {
                System.IO.File.WriteAllText(logPath, "", _encoding);
            }
        }

        public void LogDebug(string message)
        {
            var logMessage = $"[{DateTime.Now:HH:mm:ss.fff}] {message}";
            System.Diagnostics.Debug.WriteLine(logMessage);
            
            try
            {
                System.IO.File.AppendAllText(_logPath, logMessage + Environment.NewLine, _encoding);
            }
            catch
            {
                // Ignoruj błędy zapisu do pliku
            }
        }
    }
} 