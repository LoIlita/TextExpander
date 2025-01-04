namespace TextExpander.Interfaces
{
    /// <summary>
    /// Interfejs definiujący podstawowe operacje logowania w aplikacji.
    /// Umożliwia rejestrowanie zdarzeń debugowania w systemie.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Zapisuje wiadomość debugowania do logu.
        /// </summary>
        /// <param name="message">Treść wiadomości do zalogowania</param>
        void LogDebug(string message);
    }
} 