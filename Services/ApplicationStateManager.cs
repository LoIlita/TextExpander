using System;
using TextExpander.Interfaces;

namespace TextExpander.Services
{
    /// <summary>
    /// Klasa zarządzająca stanem aplikacji, w szczególności trybem nasłuchiwania.
    /// Kontroluje włączanie i wyłączanie nasłuchiwania zdarzeń klawiatury.
    /// </summary>
    public class ApplicationStateManager
    {
        private readonly ILogger _logger;
        private bool _isListening;

        /// <summary>
        /// Zdarzenie wywoływane przy zmianie stanu nasłuchiwania.
        /// </summary>
        public event Action<bool>? ListeningStateChanged;

        /// <summary>
        /// Interfejs do obsługi zdarzeń klawiatury.
        /// </summary>
        public IKeyboardHook KeyboardHook { get; }

        /// <summary>
        /// Określa, czy aplikacja jest w trybie nasłuchiwania.
        /// </summary>
        public bool IsListening
        {
            get => _isListening;
            private set
            {
                if (_isListening != value)
                {
                    _isListening = value;
                    ListeningStateChanged?.Invoke(value);
                }
            }
        }

        /// <summary>
        /// Inicjalizuje nową instancję klasy ApplicationStateManager.
        /// </summary>
        /// <param name="keyboardHook">Interfejs do obsługi zdarzeń klawiatury</param>
        /// <param name="logger">Logger do rejestrowania zdarzeń</param>
        public ApplicationStateManager(IKeyboardHook keyboardHook, ILogger logger)
        {
            KeyboardHook = keyboardHook;
            _logger = logger;
            _isListening = false;
        }

        /// <summary>
        /// Przełącza stan nasłuchiwania aplikacji.
        /// </summary>
        public void ToggleListening()
        {
            if (IsListening)
            {
                StopListening();
            }
            else
            {
                StartListening();
            }
        }

        /// <summary>
        /// Rozpoczyna nasłuchiwanie zdarzeń klawiatury.
        /// </summary>
        public void StartListening()
        {
            _logger.LogDebug("Starting keyboard hook");
            KeyboardHook.StartListening();
            IsListening = true;
        }

        /// <summary>
        /// Zatrzymuje nasłuchiwanie zdarzeń klawiatury.
        /// </summary>
        public void StopListening()
        {
            _logger.LogDebug("Stopping keyboard hook");
            KeyboardHook.StopListening();
            IsListening = false;
        }
    }
} 