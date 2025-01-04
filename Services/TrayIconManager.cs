using System;
using System.Drawing;
using System.Windows.Forms;

namespace TextExpander.Services
{
    public class TrayIconManager : IDisposable
    {
        private readonly NotifyIcon _trayIcon;
        private readonly Form _parentForm;

        public TrayIconManager(Form parentForm, Icon? icon)
        {
            _parentForm = parentForm ?? throw new ArgumentNullException(nameof(parentForm));
            
            if (icon == null)
            {
                // Użyj domyślnej ikony aplikacji, jeśli nie podano własnej
                icon = SystemIcons.Application;
            }

            _trayIcon = new NotifyIcon
            {
                Icon = icon,
                Text = "TextExpander",
                Visible = true
            };
            SetupContextMenu();
        }

        private void SetupContextMenu()
        {
            var contextMenu = new ContextMenuStrip();
            var showItem = new ToolStripMenuItem("Pokaż");
            showItem.Click += ShowForm;

            var exitItem = new ToolStripMenuItem("Zakończ");
            exitItem.Click += ExitApplication;

            contextMenu.Items.Add(showItem);
            contextMenu.Items.Add(exitItem);
            _trayIcon.ContextMenuStrip = contextMenu;
            _trayIcon.DoubleClick += ShowForm;
        }

        private void ShowForm(object? sender, EventArgs e)
        {
            _parentForm.Show();
            _parentForm.WindowState = FormWindowState.Normal;
        }

        private void ExitApplication(object? sender, EventArgs e)
        {
            _trayIcon.Visible = false;
            Application.Exit();
        }

        public void Dispose()
        {
            _trayIcon?.Dispose();
        }
    }
} 