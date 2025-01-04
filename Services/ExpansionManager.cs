using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using TextExpander.Interfaces;
using TextExpander.Settings;

namespace TextExpander.Services
{
    /// <summary>
    /// Klasa odpowiedzialna za rozwijanie skrótów tekstowych.
    /// Obsługuje przechwytywanie klawiszy i zamianę skrótów na ich rozwinięcia.
    /// </summary>
    public class ExpansionManager : IDisposable
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private const int KEYEVENTF_KEYUP = 0x0002;
        private const byte VK_BACK = 0x08;    // Backspace
        private const byte VK_CONTROL = 0x11;  // Ctrl
        private const byte VK_V = 0x56;       // V
        private const byte VK_SPACE = 0x20;   // Space
        private const byte VK_LEFT = 0x25;    // Left Arrow
        private const byte VK_ESCAPE = 0x1B;  // Escape

        // Optymalne opóźnienia dla różnych operacji
        private const int KEYBOARD_DELAY = 20;     // Podstawowe opóźnienie dla klawiszy
        private const int CLIPBOARD_DELAY = 50;    // Opóźnienie dla operacji schowka
        private const int EXPANSION_DELAY = 30;    // Opóźnienie między znakami rozwinięcia

        private readonly IShortcutManager _shortcutManager;
        private readonly ILogger _logger;
        private readonly AppSettings _settings;
        private readonly StringBuilder _currentShortcut;
        private readonly ClipboardManager _clipboardManager;
        private readonly Dictionary<string, string> _expansionCache;
        private bool _isInShortcutMode;

        public ExpansionManager(IShortcutManager shortcutManager, ILogger logger, AppSettings settings)
        {
            _shortcutManager = shortcutManager;
            _logger = logger;
            _settings = settings;
            _currentShortcut = new StringBuilder();
            _clipboardManager = new ClipboardManager();
            _expansionCache = new Dictionary<string, string>();
            _isInShortcutMode = false;
        }

        public bool ProcessKeyPress(KeyEventArgs e)
        {
            _logger.LogDebug($"[ExpansionManager] Otrzymano klawisz: {e.KeyCode}, Control: {e.Control}, Alt: {e.Alt}, Shift: {e.Shift}");
            
            if (_isInShortcutMode)
            {
                _logger.LogDebug($"[ExpansionManager] Aktualny skrót: '{_currentShortcut}'");
            }

            // Sprawdź czy wciśnięto Escape w trybie skrótu
            if (_isInShortcutMode && e.KeyCode == Keys.Escape)
            {
                CancelShortcutMode();
                e.Handled = true;
                e.SuppressKeyPress = true;
                return true;
            }

            // Sprawdź czy wciśnięto zdefiniowany hotkey
            bool isHotkeyPressed = _settings.IsTriggered(e);

            if (isHotkeyPressed)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                _isInShortcutMode = true;
                _currentShortcut.Clear();
                _logger.LogDebug("[ExpansionManager] >>> Aktywowano tryb skrótu");
                return true;
            }

            if (!_isInShortcutMode)
            {
                return false;
            }

            if (e.KeyCode == Keys.Space)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                ProcessSpacePress();
                return true;
            }

            if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
            {
                e.Handled = false;
                e.SuppressKeyPress = false;
                _currentShortcut.Append(char.ToLower((char)e.KeyCode));
                _logger.LogDebug($"[ExpansionManager] >>> Dodano literę, aktualny skrót: '{_currentShortcut}'");
                return true;
            }

            if (e.KeyCode == Keys.Back && _currentShortcut.Length > 0)
            {
                e.Handled = false;
                e.SuppressKeyPress = false;
                _currentShortcut.Length--;
                _logger.LogDebug($"[ExpansionManager] >>> Usunięto znak, aktualny skrót: '{_currentShortcut}'");
                return true;
            }

            return false;
        }

        private void ProcessSpacePress()
        {
            string shortcutToExpand = _currentShortcut.ToString();
            try
            {
                _logger.LogDebug($"[ExpansionManager] Sprawdzam skrót: '{shortcutToExpand}'");
                
                // Sprawdź cache przed odpytaniem managera
                string? expansion = null;
                if (!_expansionCache.TryGetValue(shortcutToExpand, out expansion))
                {
                    expansion = _shortcutManager.GetShortcutValue(shortcutToExpand);
                    if (!string.IsNullOrEmpty(expansion))
                    {
                        _expansionCache[shortcutToExpand] = expansion;
                    }
                }
                
                if (!string.IsNullOrEmpty(expansion))
                {
                    _logger.LogDebug($"[ExpansionManager] >>> Znaleziono rozwinięcie: '{expansion}'");
                    
                    _clipboardManager.ExecuteWithClipboard(() =>
                    {
                        // Usuń wpisany skrót
                        for (int i = 0; i < shortcutToExpand.Length; i++)
                        {
                            SimulateBackspace();
                        }
                        
                        // Wstaw rozwinięcie przez schowek
                        if (_clipboardManager.SetText(expansion))
                        {
                            System.Threading.Thread.Sleep(CLIPBOARD_DELAY);
                            SimulateCtrlV();
                            SimulateSpace();
                        }
                        else
                        {
                            _logger.LogDebug("[ExpansionManager] !!! Błąd podczas ustawiania schowka");
                        }
                    });
                }
                else
                {
                    _logger.LogDebug("[ExpansionManager] >>> Nie znaleziono rozwinięcia");
                    SimulateSpace();
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"[ExpansionManager] !!! Błąd podczas rozwijania: {ex.Message}");
                SimulateSpace();
            }
            finally
            {
                ResetShortcutMode();
            }
        }

        private void CancelShortcutMode()
        {
            _logger.LogDebug("[ExpansionManager] >>> Anulowano tryb skrótu");
            SimulateSpace();
            ResetShortcutMode();
        }

        private void ResetShortcutMode()
        {
            _isInShortcutMode = false;
            _currentShortcut.Clear();
            _logger.LogDebug("[ExpansionManager] >>> Wyłączono tryb skrótu");
        }

        private void SimulateBackspace()
        {
            keybd_event(VK_BACK, 0, 0, 0);
            keybd_event(VK_BACK, 0, KEYEVENTF_KEYUP, 0);
            System.Threading.Thread.Sleep(KEYBOARD_DELAY);
        }

        private void SimulateCtrlV()
        {
            keybd_event(VK_CONTROL, 0, 0, 0);
            keybd_event(VK_V, 0, 0, 0);
            keybd_event(VK_V, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);
            System.Threading.Thread.Sleep(EXPANSION_DELAY);
        }

        private void SimulateSpace()
        {
            keybd_event(VK_SPACE, 0, 0, 0);
            keybd_event(VK_SPACE, 0, KEYEVENTF_KEYUP, 0);
            System.Threading.Thread.Sleep(KEYBOARD_DELAY);
        }

        public void Dispose()
        {
            _clipboardManager?.Dispose();
            _expansionCache?.Clear();
        }
    }
} 