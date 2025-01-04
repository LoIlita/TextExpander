using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TextExpander.Interfaces;

namespace TextExpander.Services
{
    public class ExpansionManager
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private const int KEYEVENTF_KEYUP = 0x0002;
        private const byte VK_BACK = 0x08;
        private const byte VK_CONTROL = 0x11;
        private const byte VK_V = 0x56;
        private const byte VK_SPACE = 0x20;

        private readonly IShortcutManager _shortcutManager;
        private readonly ILogger _logger;
        private bool _isInShortcutMode;
        private string _currentShortcut;

        public ExpansionManager(IShortcutManager shortcutManager, ILogger logger)
        {
            _shortcutManager = shortcutManager;
            _logger = logger;
            _isInShortcutMode = false;
            _currentShortcut = string.Empty;
        }

        public bool HandleKeyPress(KeyEventArgs e)
        {
            _logger.LogDebug($"[ExpansionManager] Otrzymano klawisz: {e.KeyCode}, Control: {e.Control}, Alt: {e.Alt}, Shift: {e.Shift}");
            _logger.LogDebug($"[ExpansionManager] Aktualny stan: TrybSkrótu={_isInShortcutMode}, AktualnySkrót='{_currentShortcut}'");

            // Sprawdź czy wciśnięto Ctrl+M
            if (e.Control && e.KeyCode == Keys.M)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                _isInShortcutMode = true;
                _currentShortcut = string.Empty;
                _logger.LogDebug("[ExpansionManager] >>> Aktywowano tryb skrótu (Ctrl+M)");
                _logger.LogDebug("[ExpansionManager] >>> Zresetowano bufor skrótu");
                _logger.LogDebug("[ExpansionManager] >>> Następne klawisze będą zbierane do skrótu");
                return true;
            }

            // Jeśli nie jesteśmy w trybie skrótu, nie obsługujemy klawisza
            if (!_isInShortcutMode)
            {
                _logger.LogDebug("[ExpansionManager] Tryb skrótu nieaktywny - ignoruję klawisz");
                return false;
            }

            // Obsługa spacji - sprawdź i zamień skrót
            if (e.KeyCode == Keys.Space)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                HandleSpacePress();
                return true;
            }

            // Zbieranie tekstu skrótu
            if (char.IsLetterOrDigit((char)e.KeyCode))
            {
                e.Handled = false;
                e.SuppressKeyPress = false;
                string keyChar = ((char)e.KeyCode).ToString().ToLower();
                _currentShortcut += keyChar;
                _logger.LogDebug($"[ExpansionManager] >>> Dodano znak '{keyChar}' do skrótu, aktualny skrót: '{_currentShortcut}'");
                return true;
            }

            // Obsługa Backspace
            if (e.KeyCode == Keys.Back && _currentShortcut.Length > 0)
            {
                e.Handled = false;
                e.SuppressKeyPress = false;
                string removedChar = _currentShortcut.Substring(_currentShortcut.Length - 1);
                _currentShortcut = _currentShortcut.Substring(0, _currentShortcut.Length - 1);
                _logger.LogDebug($"[ExpansionManager] >>> Usunięto znak '{removedChar}' ze skrótu, aktualny skrót: '{_currentShortcut}'");
                return true;
            }

            _logger.LogDebug($"[ExpansionManager] Nieobsługiwany klawisz w trybie skrótu: {e.KeyCode}");
            return false;
        }

        private void SimulateBackspace()
        {
            keybd_event(VK_BACK, 0, 0, 0);
            keybd_event(VK_BACK, 0, KEYEVENTF_KEYUP, 0);
            System.Threading.Thread.Sleep(10);
        }

        private void SimulateCtrlV()
        {
            keybd_event(VK_CONTROL, 0, 0, 0);
            keybd_event(VK_V, 0, 0, 0);
            keybd_event(VK_V, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);
            System.Threading.Thread.Sleep(50);
        }

        private void SimulateSpace()
        {
            keybd_event(VK_SPACE, 0, 0, 0);
            keybd_event(VK_SPACE, 0, KEYEVENTF_KEYUP, 0);
            System.Threading.Thread.Sleep(10);
        }

        private void HandleSpacePress()
        {
            string shortcutToExpand = _currentShortcut; // Zapamiętaj skrót przed wyczyszczeniem
            try
            {
                _logger.LogDebug($"[ExpansionManager] Sprawdzam skrót: '{shortcutToExpand}'");
                var expansion = _shortcutManager.GetShortcutValue(shortcutToExpand);
                
                if (expansion != null && !string.IsNullOrEmpty(expansion))
                {
                    _logger.LogDebug($"[ExpansionManager] >>> Znaleziono rozwinięcie: '{expansion}'");
                    
                    // Zapamiętaj aktualną zawartość schowka
                    string oldClipboard = System.Windows.Forms.Clipboard.GetText();
                    
                    try
                    {
                        // Usuń wpisany skrót
                        _logger.LogDebug($"[ExpansionManager] >>> Usuwam wpisany skrót (długość: {shortcutToExpand.Length})");
                        for (int i = 0; i < shortcutToExpand.Length; i++)
                        {
                            SimulateBackspace();
                        }
                        
                        // Wstaw rozwinięcie przez schowek
                        _logger.LogDebug($"[ExpansionManager] >>> Kopiuję rozwinięcie do schowka");
                        System.Windows.Forms.Clipboard.SetText(expansion);
                        System.Threading.Thread.Sleep(50); // Daj czas na zaktualizowanie schowka
                        
                        _logger.LogDebug($"[ExpansionManager] >>> Wklejam rozwinięcie ze schowka");
                        SimulateCtrlV();
                        
                        _logger.LogDebug($"[ExpansionManager] >>> Dodaję spację");
                        SimulateSpace();
                        
                        _logger.LogDebug("[ExpansionManager] >>> Zakończono wstawianie tekstu");
                    }
                    finally
                    {
                        // Przywróć poprzednią zawartość schowka
                        System.Threading.Thread.Sleep(50);
                        System.Windows.Forms.Clipboard.SetText(oldClipboard);
                        _logger.LogDebug("[ExpansionManager] >>> Przywrócono poprzednią zawartość schowka");
                    }
                }
                else
                {
                    _logger.LogDebug("[ExpansionManager] >>> Nie znaleziono rozwinięcia, zachowuję oryginalny tekst");
                    SimulateSpace();
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"[ExpansionManager] !!! BŁĄD podczas rozwijania: {ex.Message}");
                _logger.LogDebug($"[ExpansionManager] !!! Szczegóły błędu: {ex}");
                SimulateSpace();
            }
            finally
            {
                // Wyłącz tryb skrótu dopiero po wszystkich operacjach
                _isInShortcutMode = false;
                _currentShortcut = string.Empty;
                _logger.LogDebug("[ExpansionManager] >>> Wyłączono tryb skrótu");
                _logger.LogDebug("[ExpansionManager] >>> Wyczyszczono bufor skrótu");
            }
        }
    }
} 