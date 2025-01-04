using System;
using System.Windows.Forms;
using TextExpander.Settings;
using TextExpander.Interfaces;

namespace TextExpander.Services
{
    /// <summary>
    /// Klasa zarządzająca ustawieniami i obsługą klawiszy skrótu (hotkey) w aplikacji.
    /// Umożliwia konfigurację i monitorowanie kombinacji klawiszy aktywujących funkcje aplikacji.
    /// </summary>
    public class HotkeyManager
    {
        private readonly AppSettings _settings;
        private readonly ILogger _logger;
        private readonly Action _updateUiCallback;

        /// <summary>
        /// Inicjalizuje nową instancję klasy HotkeyManager.
        /// </summary>
        /// <param name="settings">Ustawienia aplikacji</param>
        /// <param name="logger">Logger do rejestrowania zdarzeń</param>
        /// <param name="updateUiCallback">Callback wywoływany po zmianie ustawień hotkey</param>
        public HotkeyManager(AppSettings settings, ILogger logger, Action updateUiCallback)
        {
            _settings = settings;
            _logger = logger;
            _updateUiCallback = updateUiCallback;
        }

        /// <summary>
        /// Wyświetla okno ustawień hotkey.
        /// </summary>
        /// <param name="owner">Okno nadrzędne dla formularza ustawień</param>
        public void ShowHotkeySettings(IWin32Window owner)
        {
            _logger.LogDebug("Opening hotkey settings");
            using (var form = new Forms.HotkeySettingsForm(_settings))
            {
                if (form.ShowDialog(owner) == DialogResult.OK)
                {
                    _settings.Save();
                    _updateUiCallback();
                    _logger.LogDebug($"Hotkey updated to: {_settings.GetKeyDescription()}");
                }
            }
        }

        /// <summary>
        /// Pobiera tekstowy opis aktualnie ustawionego hotkey.
        /// </summary>
        /// <returns>Opis hotkey w formacie czytelnym dla użytkownika</returns>
        public string GetHotkeyDescription()
        {
            return _settings.GetKeyDescription();
        }
    }
} 