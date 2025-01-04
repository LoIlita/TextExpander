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

        public MainForm()
        {
            InitializeComponent();

            // Inicjalizacja podstawowych serwisów
            _logger = new FileLogger("textexpander.log");
            _shortcutManager = new JsonShortcutManager("shortcuts.json");
            _settings = AppSettings.Load();

            // Inicjalizacja menedżerów
            var keyboardHook = new GlobalKeyboardHook();
            _stateManager = new ApplicationStateManager(keyboardHook, _logger);
            _expansionManager = new ExpansionManager(_shortcutManager, _logger);
            _listManager = new ShortcutListManager(listViewShortcuts, _shortcutManager);
            _trayManager = new TrayIconManager(this, Icon);
            _hotkeyManager = new HotkeyManager(_settings, _logger, UpdateHotkeyLabel);

            // Konfiguracja
            SetupEventHandlers();
            _listManager.LoadShortcuts();
            UpdateHotkeyLabel();

            _logger.LogDebug("MainForm initialization completed");
        }

        private void SetupEventHandlers()
        {
            _stateManager.ListeningStateChanged += OnListeningStateChanged;
            _stateManager.KeyboardHook.KeyDown += OnKeyDown;
        }

        private void OnListeningStateChanged(bool isListening)
        {
            btnToggleListening.Text = isListening ? "Stop Listening" : "Start Listening";
            MessageBox.Show(
                isListening ? "Nasłuchiwanie zostało włączone" : "Nasłuchiwanie zostało wyłączone",
                "Status",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (_stateManager.IsListening)
            {
                _logger.LogDebug($"[MainForm] Przechwycono klawisz: {e.KeyCode}");
                _logger.LogDebug($"[MainForm] Stan nasłuchiwania: aktywny");
                var handled = _expansionManager.HandleKeyPress(e);
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
                    _listManager.UpdateShortcut(selectedKey, form.ShortcutValue);
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
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

