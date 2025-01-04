using System;
using System.IO;
using TextExpander.Interfaces;

namespace TextExpander.Services
{
    /// <summary>
    /// Implementacja interfejsu ILogger zapisująca logi do pliku.
    /// Umożliwia rejestrowanie zdarzeń debugowania w pliku tekstowym.
    /// </summary>
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;

        /// <summary>
        /// Inicjalizuje nową instancję klasy FileLogger.
        /// </summary>
        /// <param name="logFileName">Nazwa pliku, do którego będą zapisywane logi</param>
        public FileLogger(string logFileName)
        {
            _logFilePath = logFileName;
        }

        /// <summary>
        /// Zapisuje wiadomość debugowania do pliku logu.
        /// Każda wiadomość jest poprzedzona znacznikiem czasu.
        /// </summary>
        /// <param name="message">Treść wiadomości do zalogowania</param>
        public void LogDebug(string message)
        {
            try
            {
                string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [DEBUG] {message}";
                File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
            }
            catch
            {
                // Ignorujemy błędy zapisu do pliku
            }
        }
    }
} 