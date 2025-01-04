using System;
using System.Windows.Forms;
using TextExpander.Interfaces;
using TextExpander.Services;
using TextExpander.Settings;

namespace TextExpander.Forms
{
    public partial class MainForm : Form
    {
        private readonly ILogger _logger;
        private readonly IShortcutManager _shortcutManager;
        private readonly AppSettings _settings;
        private readonly ExpansionManager _expansionManager;
        private readonly ApplicationStateManager _stateManager;
        private readonly ShortcutListManager _listManager;
        private readonly TrayIconManager _trayManager;
        private readonly HotkeyManager _hotkeyManager;
        private readonly ThemeManager _themeManager;
        private readonly WindowSettings _windowSettings;

        public MainForm()
        {
            InitializeComponent();

            // Inicjalizacja podstawowych serwisów
            _logger = new FileLogger("textexpander.log");
            _shortcutManager = new JsonShortcutManager("shortcuts.json");
            _settings = AppSettings.Load();
            _windowSettings = WindowSettings.Load();

            // Inicjalizacja menedżerów
            var keyboardHook = new GlobalKeyboardHook();
            _stateManager = new ApplicationStateManager(keyboardHook, _logger);
            _expansionManager = new ExpansionManager(_shortcutManager, _logger, _settings);
            _listManager = new ShortcutListManager(listViewShortcuts, _shortcutManager);
            _trayManager = new TrayIconManager(this, Icon);
            _hotkeyManager = new HotkeyManager(_settings, _logger, UpdateHotkeyLabel);
            _themeManager = new ThemeManager(this, _logger);

            // Konfiguracja
            SetupEventHandlers();
            _listManager.LoadShortcuts();
            UpdateHotkeyLabel();
            
            // Załaduj i zastosuj zapisany motyw
            _themeManager.ApplyTheme();
            _logger.LogDebug($"[MainForm] Załadowano motyw: {(ThemeSettings.Instance.IsDarkMode ? "ciemny" : "jasny")}");
            
            ApplyWindowSettings();

            _logger.LogDebug("MainForm initialization completed");
        }

        private void SetupEventHandlers()
        {
            _stateManager.ListeningStateChanged += OnListeningStateChanged;
            _stateManager.KeyboardHook.KeyDown += OnKeyDown;
            _themeManager.ThemeChanged += OnThemeChanged;
        }

        private void OnThemeChanged()
        {
            _logger.LogDebug("[MainForm] Zaktualizowano motyw");
        }

        private void BtnChangeTheme_Click(object? sender, EventArgs e)
        {
            _logger.LogDebug("[MainForm] Kliknięto przycisk zmiany motywu");
            _themeManager.ToggleTheme();
        }

        private void OnListeningStateChanged(bool isListening)
        {
            btnToggleListening.Text = isListening ? "Stop Listening" : "Start Listening";
            
            if (isListening)
            {
                btnToggleListening.BackColor = System.Drawing.Color.LightGreen;
                btnToggleListening.ForeColor = System.Drawing.Color.DarkGreen;
                btnToggleListening.Font = new System.Drawing.Font(btnToggleListening.Font, System.Drawing.FontStyle.Bold);
            }
            else
            {
                // Przywróć kolory zgodne z aktualnym motywem
                _themeManager.ApplyTheme();
                btnToggleListening.Font = new System.Drawing.Font(btnToggleListening.Font, System.Drawing.FontStyle.Regular);
            }
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (_stateManager.IsListening)
            {
                _logger.LogDebug($"[MainForm] Przechwycono klawisz: {e.KeyCode}");
                _logger.LogDebug($"[MainForm] Stan nasłuchiwania: aktywny");
                _logger.LogDebug($"[MainForm] Hotkey: {_settings.GetKeyDescription()}");
                _logger.LogDebug($"[MainForm] Control: {e.Control}, Alt: {e.Alt}, Shift: {e.Shift}");
                
                var handled = _expansionManager.ProcessKeyPress(e);
                _logger.LogDebug($"[MainForm] Klawisz {(handled ? "został obsłużony" : "nie został obsłużony")}");
            }
            else
            {
                _logger.LogDebug("[MainForm] Pominięto klawisz - nasłuchiwanie nieaktywne");
            }
        }

        private void UpdateHotkeyLabel()
        {
            var hotkey = _hotkeyManager.GetHotkeyDescription();
            _logger.LogDebug($"[MainForm] Aktualizacja etykiety hotkey: {hotkey}");
            lblHotkey.Text = $"Hotkey: {hotkey}";
        }

        private void BtnToggleListening_Click(object? sender, EventArgs e)
        {
            _logger.LogDebug("[MainForm] Kliknięto przycisk przełączania nasłuchiwania");
            _stateManager.ToggleListening();
            _logger.LogDebug($"[MainForm] Stan nasłuchiwania po przełączeniu: {(_stateManager.IsListening ? "aktywny" : "nieaktywny")}");
        }

        private void BtnChangeHotkey_Click(object? sender, EventArgs e)
        {
            _logger.LogDebug("[MainForm] Kliknięto przycisk zmiany hotkey");
            _hotkeyManager.ShowHotkeySettings(this);
        }

        private void BtnAddShortcut_Click(object? sender, EventArgs e)
        {
            _logger.LogDebug("[MainForm] Kliknięto przycisk dodawania skrótu");
            using (var form = new ShortcutForm())
            {
                _logger.LogDebug("[MainForm] Otwarto formularz dodawania skrótu");
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _logger.LogDebug($"[MainForm] Próba dodania skrótu: '{form.ShortcutKey}' -> '{form.ShortcutValue}'");
                    if (!_listManager.AddShortcut(form.ShortcutKey, form.ShortcutValue))
                    {
                        _logger.LogDebug("[MainForm] !!! Błąd: skrót już istnieje");
                        MessageBox.Show("Shortcut already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        _logger.LogDebug("[MainForm] Skrót został dodany pomyślnie");
                    }
                }
                else
                {
                    _logger.LogDebug("[MainForm] Anulowano dodawanie skrótu");
                }
            }
        }

        private void BtnEditShortcut_Click(object? sender, EventArgs e)
        {
            var selectedKey = _listManager.GetSelectedShortcut();
            if (selectedKey == null) return;

            var currentValue = _shortcutManager.GetShortcutValue(selectedKey);
            using (var form = new ShortcutForm(selectedKey, currentValue))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _listManager.UpdateShortcut(selectedKey, form.ShortcutKey, form.ShortcutValue);
                }
            }
        }

        private void BtnDeleteShortcut_Click(object? sender, EventArgs e)
        {
            var selectedKey = _listManager.GetSelectedShortcut();
            if (selectedKey == null) return;

            if (MessageBox.Show(
                $"Are you sure you want to delete shortcut '{selectedKey}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _listManager.DeleteShortcut(selectedKey);
            }
        }

        private void ApplyWindowSettings()
        {
            if (_windowSettings.IsMaximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                Location = _windowSettings.Location;
                Size = _windowSettings.Size;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                // Zapisz ustawienia okna
                _windowSettings.IsMaximized = (WindowState == FormWindowState.Maximized);
                if (WindowState == FormWindowState.Normal)
                {
                    _windowSettings.Location = Location;
                    _windowSettings.Size = Size;
                }
                _windowSettings.Save();

                // Zapisz pozostałe ustawienia
                _stateManager.StopListening();
                _settings.Save();
                _shortcutManager.SaveShortcuts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Wystąpił błąd podczas zamykania aplikacji: {ex.Message}",
                    "Błąd",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                base.OnFormClosing(e);
            }
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            if (WindowState == FormWindowState.Normal)
            {
                _windowSettings.Size = Size;
                _windowSettings.Save();
            }
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            if (WindowState == FormWindowState.Normal)
            {
                _windowSettings.Location = Location;
                _windowSettings.Save();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
            base.OnSizeChanged(e);
        }
    }
} 

