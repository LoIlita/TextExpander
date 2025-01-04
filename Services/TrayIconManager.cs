using System;
using System.Drawing;
using System.Windows.Forms;

namespace TextExpander.Services
{
    /// <summary>
    /// Klasa zarządzająca ikoną w zasobniku systemowym.
    /// Umożliwia interakcję z aplikacją poprzez menu kontekstowe w zasobniku.
    /// </summary>
    public class TrayIconManager : IDisposable
    {
        private readonly NotifyIcon _trayIcon;
        private readonly Form _mainForm;

        /// <summary>
        /// Inicjalizuje nową instancję klasy TrayIconManager.
        /// </summary>
        /// <param name="mainForm">Główne okno aplikacji</param>
        /// <param name="icon">Ikona do wyświetlenia w zasobniku. Jeśli null, zostanie użyta domyślna ikona aplikacji.</param>
        /// <exception cref="ArgumentNullException">Rzucany gdy mainForm jest null</exception>
        public TrayIconManager(Form mainForm, Icon? icon)
        {
            _mainForm = mainForm ?? throw new ArgumentNullException(nameof(mainForm));
            
            _trayIcon = new NotifyIcon
            {
                Icon = icon ?? SystemIcons.Application,
                Text = "TextExpander",
                Visible = true,
                ContextMenuStrip = CreateContextMenu()
            };

            _trayIcon.DoubleClick += TrayIcon_DoubleClick;
        }

        /// <summary>
        /// Tworzy menu kontekstowe dla ikony w zasobniku.
        /// </summary>
        /// <returns>Menu kontekstowe z opcjami aplikacji</returns>
        private ContextMenuStrip CreateContextMenu()
        {
            var menu = new ContextMenuStrip();
            var showItem = new ToolStripMenuItem("Pokaż", null, ShowItem_Click);
            var exitItem = new ToolStripMenuItem("Zakończ", null, ExitItem_Click);

            menu.Items.Add(showItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(exitItem);

            return menu;
        }

        /// <summary>
        /// Obsługuje zdarzenie kliknięcia w opcję "Pokaż" w menu kontekstowym.
        /// </summary>
        private void ShowItem_Click(object? sender, EventArgs e)
        {
            _mainForm.Show();
            _mainForm.WindowState = FormWindowState.Normal;
            _mainForm.Activate();
        }

        /// <summary>
        /// Obsługuje zdarzenie kliknięcia w opcję "Zakończ" w menu kontekstowym.
        /// </summary>
        private void ExitItem_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Obsługuje zdarzenie podwójnego kliknięcia w ikonę w zasobniku.
        /// </summary>
        private void TrayIcon_DoubleClick(object? sender, EventArgs e)
        {
            ShowItem_Click(sender, e);
        }

        /// <summary>
        /// Zwalnia zasoby używane przez ikonę w zasobniku.
        /// </summary>
        public void Dispose()
        {
            _trayIcon.Dispose();
        }
    }
} 