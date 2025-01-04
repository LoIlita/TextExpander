using System;
using TextExpander.Interfaces;

namespace TextExpander.Services
{
    public class ApplicationStateManager
    {
        private readonly IKeyboardHook _keyboardHook;
        private readonly ILogger _logger;
        private bool _isListening;

        public event Action<bool>? ListeningStateChanged;
        public IKeyboardHook KeyboardHook => _keyboardHook;

        public ApplicationStateManager(IKeyboardHook keyboardHook, ILogger logger)
        {
            _keyboardHook = keyboardHook;
            _logger = logger;
            _isListening = false;
        }

        public void ToggleListening()
        {
            _isListening = !_isListening;
            _logger.LogDebug($"[StateManager] >>> Zmiana stanu nasłuchiwania na: {_isListening}");
            
            if (_isListening)
            {
                _logger.LogDebug("[StateManager] >>> Uruchamiam przechwytywanie klawiatury");
                _keyboardHook.StartHook();
                _logger.LogDebug("[StateManager] >>> Przechwytywanie klawiatury aktywne");
            }
            else
            {
                _logger.LogDebug("[StateManager] >>> Zatrzymuję przechwytywanie klawiatury");
                _keyboardHook.StopHook();
                _logger.LogDebug("[StateManager] >>> Przechwytywanie klawiatury zatrzymane");
            }

            _logger.LogDebug("[StateManager] >>> Powiadamiam o zmianie stanu");
            ListeningStateChanged?.Invoke(_isListening);
            _logger.LogDebug("[StateManager] >>> Zakończono zmianę stanu");
        }

        public void StopListening()
        {
            if (_isListening)
            {
                _logger.LogDebug("[StateManager] Wymuszenie zatrzymania nasłuchiwania");
                _keyboardHook.StopHook();
                _isListening = false;
                _logger.LogDebug("[StateManager] >>> Przechwytywanie klawiatury zatrzymane");
                _logger.LogDebug("[StateManager] >>> Stan nasłuchiwania: nieaktywny");
                ListeningStateChanged?.Invoke(false);
                _logger.LogDebug("[StateManager] >>> Powiadomiono o zmianie stanu");
            }
            else
            {
                _logger.LogDebug("[StateManager] Próba zatrzymania nasłuchiwania, ale było już nieaktywne");
            }
        }

        public bool IsListening => _isListening;
    }
} 