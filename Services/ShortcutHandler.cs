using System;
using System.Windows.Forms;
using TextExpander.Interfaces;

namespace TextExpander.Services
{
    public class ShortcutHandler
    {
        private readonly IShortcutManager _shortcutManager;
        private bool _isInShortcutMode;
        private string _currentShortcut;
        private readonly IKeyboardHook _keyboardHook;

        public IKeyboardHook KeyboardHook => _keyboardHook;

        public ShortcutHandler(IShortcutManager shortcutManager, IKeyboardHook keyboardHook)
        {
            _shortcutManager = shortcutManager;
            _keyboardHook = keyboardHook;
            _isInShortcutMode = false;
            _currentShortcut = string.Empty;
        }

        public bool HandleKeyPress(KeyEventArgs e, Action<string> logAction)
        {
            // Sprawdź czy wciśnięto Ctrl+M
            if (e.Control && e.KeyCode == Keys.M)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                _isInShortcutMode = true;
                _currentShortcut = string.Empty;
                logAction("Shortcut mode activated (Ctrl+M pressed)");
                return true;
            }

            // Jeśli nie jesteśmy w trybie skrótu, nie obsługujemy klawisza
            if (!_isInShortcutMode)
                return false;

            // Obsługa spacji - sprawdź i zamień skrót
            if (e.KeyCode == Keys.Space)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                HandleSpacePress(logAction);
                return true;
            }

            // Zbieranie tekstu skrótu
            if (char.IsLetterOrDigit((char)e.KeyCode))
            {
                e.Handled = false;
                e.SuppressKeyPress = false;
                string keyChar = ((char)e.KeyCode).ToString().ToLower();
                _currentShortcut += keyChar;
                logAction($"Current shortcut: '{_currentShortcut}'");
                return true;
            }

            // Obsługa Backspace
            if (e.KeyCode == Keys.Back && _currentShortcut.Length > 0)
            {
                e.Handled = false;
                e.SuppressKeyPress = false;
                _currentShortcut = _currentShortcut.Substring(0, _currentShortcut.Length - 1);
                logAction($"Backspace pressed, current shortcut: '{_currentShortcut}'");
                return true;
            }

            return false;
        }

        private void HandleSpacePress(Action<string> logAction)
        {
            try
            {
                logAction($"Checking shortcut: '{_currentShortcut}'");
                var expansion = _shortcutManager.GetShortcutValue(_currentShortcut);
                if (expansion != null && !string.IsNullOrEmpty(expansion))
                {
                    logAction($"Found expansion: '{expansion}'");
                    
                    // Usuń wpisany skrót
                    for (int i = 0; i < _currentShortcut.Length; i++)
                    {
                        SendKeys.SendWait("{BACKSPACE}");
                    }
                    
                    // Wstaw rozwinięcie ze spacją
                    foreach (char c in expansion)
                    {
                        SendKeys.SendWait(c.ToString());
                    }
                    SendKeys.SendWait(" ");
                    logAction("Expansion inserted with space");
                }
                else
                {
                    logAction("No expansion found, keeping original text");
                    SendKeys.SendWait(" ");
                }
            }
            catch (Exception ex)
            {
                logAction($"Error during expansion: {ex.Message}");
                SendKeys.SendWait(" ");
            }

            // Wyłącz tryb skrótu
            _isInShortcutMode = false;
            _currentShortcut = string.Empty;
            logAction("Shortcut mode deactivated");
        }
    }
} 