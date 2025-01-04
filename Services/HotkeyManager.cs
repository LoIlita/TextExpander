using System;
using System.Windows.Forms;
using TextExpander.Settings;
using TextExpander.Interfaces;

namespace TextExpander.Services
{
    public class HotkeyManager
    {
        private readonly AppSettings _settings;
        private readonly ILogger _logger;
        private readonly Action _updateUiCallback;

        public HotkeyManager(AppSettings settings, ILogger logger, Action updateUiCallback)
        {
            _settings = settings;
            _logger = logger;
            _updateUiCallback = updateUiCallback;
        }

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

        public string GetHotkeyDescription()
        {
            return _settings.GetKeyDescription();
        }
    }
} 