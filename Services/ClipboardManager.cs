using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace TextExpander.Services
{
    /// <summary>
    /// Klasa zarządzająca operacjami na schowku systemowym.
    /// Zapewnia bezpieczne i wydajne operacje kopiowania i wklejania tekstu.
    /// </summary>
    public class ClipboardManager : IDisposable
    {
        private const int MaxRetries = 5;
        private const int RetryDelay = 10;
        private string? _savedClipboard;
        private readonly object _lockObject = new object();

        /// <summary>
        /// Wykonuje operację na schowku z zachowaniem jego poprzedniej zawartości.
        /// </summary>
        /// <param name="action">Akcja do wykonania ze schowkiem</param>
        public void ExecuteWithClipboard(Action action)
        {
            lock (_lockObject)
            {
                try
                {
                    SaveClipboard();
                    action();
                }
                finally
                {
                    RestoreClipboard();
                }
            }
        }

        /// <summary>
        /// Bezpiecznie kopiuje tekst do schowka z obsługą ponownych prób.
        /// </summary>
        /// <param name="text">Tekst do skopiowania</param>
        /// <returns>True jeśli operacja się powiodła</returns>
        public bool SetText(string text)
        {
            for (int i = 0; i < MaxRetries; i++)
            {
                try
                {
                    Clipboard.SetText(text);
                    return true;
                }
                catch (ExternalException)
                {
                    if (i < MaxRetries - 1)
                    {
                        Thread.Sleep(RetryDelay);
                        continue;
                    }
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Bezpiecznie pobiera tekst ze schowka z obsługą ponownych prób.
        /// </summary>
        /// <returns>Pobrany tekst lub pusty string w przypadku błędu</returns>
        public string GetText()
        {
            for (int i = 0; i < MaxRetries; i++)
            {
                try
                {
                    if (Clipboard.ContainsText())
                    {
                        return Clipboard.GetText();
                    }
                    return string.Empty;
                }
                catch (ExternalException)
                {
                    if (i < MaxRetries - 1)
                    {
                        Thread.Sleep(RetryDelay);
                        continue;
                    }
                }
            }
            return string.Empty;
        }

        private void SaveClipboard()
        {
            try
            {
                _savedClipboard = GetText();
            }
            catch
            {
                _savedClipboard = null;
            }
        }

        private void RestoreClipboard()
        {
            if (_savedClipboard != null)
            {
                try
                {
                    SetText(_savedClipboard);
                }
                catch
                {
                    // Ignorujemy błędy przy przywracaniu schowka
                }
            }
        }

        public void Dispose()
        {
            RestoreClipboard();
        }
    }
} 