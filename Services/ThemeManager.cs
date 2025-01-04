using System;
using System.Windows.Forms;
using System.Drawing;
using TextExpander.Settings;
using TextExpander.Interfaces;

namespace TextExpander.Services
{
    /// <summary>
    /// Klasa odpowiedzialna za zarządzanie motywem aplikacji.
    /// Implementuje wzorzec obserwatora do powiadamiania o zmianach motywu.
    /// </summary>
    public class ThemeManager
    {
        private readonly ILogger _logger;
        private readonly Form _mainForm;
        private readonly ThemeSettings _settings;

        /// <summary>
        /// Zdarzenie wywoływane przy zmianie motywu.
        /// </summary>
        public event Action? ThemeChanged;

        /// <summary>
        /// Inicjalizuje nową instancję klasy ThemeManager.
        /// </summary>
        /// <param name="mainForm">Główne okno aplikacji</param>
        /// <param name="logger">Logger do rejestrowania zdarzeń</param>
        public ThemeManager(Form mainForm, ILogger logger)
        {
            _mainForm = mainForm;
            _logger = logger;
            _settings = ThemeSettings.Instance;
        }

        /// <summary>
        /// Przełącza między jasnym a ciemnym motywem.
        /// </summary>
        public void ToggleTheme()
        {
            _logger.LogDebug($"[ThemeManager] Przełączanie motywu z {(_settings.IsDarkMode ? "ciemnego" : "jasnego")}");
            _settings.IsDarkMode = !_settings.IsDarkMode;
            _settings.Save();
            ApplyTheme();
            _logger.LogDebug($"[ThemeManager] Przełączono na motyw {(_settings.IsDarkMode ? "ciemny" : "jasny")}");
        }

        /// <summary>
        /// Aplikuje aktualny motyw do formularza.
        /// </summary>
        public void ApplyTheme()
        {
            _logger.LogDebug("[ThemeManager] Aplikowanie motywu");
            
            // Aplikuj kolory do głównego formularza
            _mainForm.BackColor = _settings.BackgroundColor;
            _mainForm.ForeColor = _settings.TextColor;

            // Aplikuj kolory do wszystkich kontrolek
            ApplyThemeToControls(_mainForm.Controls);
            
            // Powiadom obserwatorów o zmianie motywu
            ThemeChanged?.Invoke();
            
            _logger.LogDebug("[ThemeManager] Motyw został zaaplikowany");
        }

        private void ApplyThemeToControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                // Ustaw kolory w zależności od typu kontrolki
                if (control is Button button)
                {
                    button.BackColor = _settings.ButtonBackgroundColor;
                    button.ForeColor = _settings.HeaderColor;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = _settings.BorderColor;
                    button.FlatAppearance.MouseOverBackColor = _settings.AccentColor;
                }
                else if (control is ListView listView)
                {
                    listView.BackColor = _settings.ListBackgroundColor1;
                    listView.ForeColor = _settings.TextColor;
                    listView.BorderStyle = BorderStyle.FixedSingle;
                    listView.GridLines = true;
                    listView.OwnerDraw = true;

                    // Dodaj obsługę rysowania wierszy (efekt zebry)
                    listView.DrawItem -= ListView_DrawItem;
                    listView.DrawItem += ListView_DrawItem;
                    listView.DrawColumnHeader -= ListView_DrawColumnHeader;
                    listView.DrawColumnHeader += ListView_DrawColumnHeader;
                    listView.DrawSubItem -= ListView_DrawSubItem;
                    listView.DrawSubItem += ListView_DrawSubItem;
                }
                else if (control is Label)
                {
                    control.BackColor = _settings.BackgroundColor;
                    control.ForeColor = _settings.HeaderColor;
                }
                else
                {
                    control.BackColor = _settings.ListBackgroundColor1;
                    control.ForeColor = _settings.TextColor;
                }

                // Jeśli kontrolka ma własne kontrolki, zastosuj motyw rekurencyjnie
                if (control.HasChildren)
                {
                    ApplyThemeToControls(control.Controls);
                }
            }
        }

        private void ListView_DrawColumnHeader(object? sender, DrawListViewColumnHeaderEventArgs e)
        {
            if (e.Header == null || e.Graphics == null) return;

            using var bgBrush = new SolidBrush(_settings.ButtonBackgroundColor);
            using var textBrush = new SolidBrush(_settings.HeaderColor);
            using var borderPen = new Pen(_settings.BorderColor);

            e.Graphics.FillRectangle(bgBrush, e.Bounds);
            
            if (e.Font != null)
            {
                e.Graphics.DrawString(e.Header.Text, e.Font, textBrush,
                    new Rectangle(e.Bounds.X + 3, e.Bounds.Y + 2, e.Bounds.Width - 6, e.Bounds.Height - 4));
            }
            
            e.Graphics.DrawLine(borderPen, e.Bounds.X, e.Bounds.Bottom - 1,
                e.Bounds.Right, e.Bounds.Bottom - 1);
            e.Graphics.DrawLine(borderPen, e.Bounds.Right - 1, e.Bounds.Y,
                e.Bounds.Right - 1, e.Bounds.Bottom);
        }

        private void ListView_DrawItem(object? sender, DrawListViewItemEventArgs e)
        {
            if (e.Item == null || e.Graphics == null) return;

            e.DrawDefault = false;
            var bgColor = e.ItemIndex % 2 == 0 ? _settings.ListBackgroundColor1 : _settings.ListBackgroundColor2;
            
            using var bgBrush = new SolidBrush(bgColor);
            e.Graphics.FillRectangle(bgBrush, e.Bounds);
            e.Item.ForeColor = _settings.TextColor;
        }

        private void ListView_DrawSubItem(object? sender, DrawListViewSubItemEventArgs e)
        {
            if (e.SubItem == null || e.Graphics == null || e.SubItem.Font == null) return;

            e.DrawDefault = false;
            var bgColor = e.ItemIndex % 2 == 0 ? _settings.ListBackgroundColor1 : _settings.ListBackgroundColor2;
            
            // Jeśli element jest zaznaczony, użyj koloru akcentu
            if (e.Item?.Selected == true)
            {
                bgColor = _settings.AccentColor;
            }
            // Jeśli myszka jest nad elementem, rozjaśnij kolor tła
            else if (e.Item?.Bounds.Contains((sender as ListView)?.PointToClient(Control.MousePosition) ?? Point.Empty) == true)
            {
                bgColor = _settings.IsDarkMode ? 
                    Color.FromArgb(bgColor.R + 20, bgColor.G + 20, bgColor.B + 20) : 
                    Color.FromArgb(bgColor.R - 20, bgColor.G - 20, bgColor.B - 20);
            }
            
            using var bgBrush = new SolidBrush(bgColor);
            using var textBrush = new SolidBrush(e.Item?.Selected == true ? Color.White : _settings.TextColor);
            
            e.Graphics.FillRectangle(bgBrush, e.Bounds);
            e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, textBrush,
                new Rectangle(e.Bounds.X + 3, e.Bounds.Y + 2, e.Bounds.Width - 6, e.Bounds.Height - 4));
        }
    }
} 