using System;
using System.Drawing;
using System.Windows.Forms;
using TextExpander.Interfaces;

namespace TextExpander.Forms
{
    public partial class ShortcutListForm : Form
    {
        private readonly ListBox listBoxShortcuts;
        private readonly IShortcutManager _shortcutManager;
        private readonly TextBox txtFilter;

        public string? SelectedShortcut { get; private set; }

        public ShortcutListForm(IShortcutManager shortcutManager, Point location)
        {
            _shortcutManager = shortcutManager;
            
            // Inicjalizacja kontrolek
            txtFilter = new TextBox
            {
                Dock = DockStyle.Top,
                Height = 25,
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            txtFilter.TextChanged += TxtFilter_TextChanged;

            listBoxShortcuts = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Segoe UI", 12F)
            };
            listBoxShortcuts.DoubleClick += ListBoxShortcuts_DoubleClick;
            listBoxShortcuts.KeyDown += ListBoxShortcuts_KeyDown;

            // Konfiguracja formularza
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.ShowInTaskbar = false;
            this.Size = new System.Drawing.Size(300, 400);
            this.KeyPreview = true;
            this.TopMost = true;
            this.Location = location;

            // Dodanie kontrolek
            Controls.Add(listBoxShortcuts);
            Controls.Add(txtFilter);

            // Inicjalne załadowanie listy
            RefreshList();

            // Ustawienie fokusa
            this.Load += (s, e) => txtFilter.Focus();

            // Obsługa klawiszy
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                    e.Handled = true;
                }
            };

            txtFilter.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Down && listBoxShortcuts.Items.Count > 0)
                {
                    listBoxShortcuts.Focus();
                    listBoxShortcuts.SelectedIndex = 0;
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Enter && listBoxShortcuts.Items.Count > 0)
                {
                    if (listBoxShortcuts.SelectedIndex == -1)
                        listBoxShortcuts.SelectedIndex = 0;
                    SelectCurrentItem();
                    e.Handled = true;
                }
            };

            listBoxShortcuts.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SelectCurrentItem();
                    e.Handled = true;
                }
                else if (char.IsLetterOrDigit((char)e.KeyCode))
                {
                    txtFilter.Focus();
                    txtFilter.Text += e.KeyCode.ToString().ToLower();
                    txtFilter.SelectionStart = txtFilter.Text.Length;
                    e.Handled = true;
                }
            };

            // Zamknij przy utracie fokusa
            this.Deactivate += (s, e) => this.Close();
        }

        private void TxtFilter_TextChanged(object? sender, EventArgs e)
        {
            RefreshList();
            if (listBoxShortcuts.Items.Count > 0 && listBoxShortcuts.SelectedIndex == -1)
            {
                listBoxShortcuts.SelectedIndex = 0;
            }
        }

        private void RefreshList()
        {
            listBoxShortcuts.Items.Clear();
            var filter = txtFilter.Text.ToLower();
            
            foreach (var shortcut in _shortcutManager.GetAllShortcuts())
            {
                if (string.IsNullOrEmpty(filter) || 
                    shortcut.Key.ToLower().Contains(filter) || 
                    shortcut.Value.ToLower().Contains(filter))
                {
                    listBoxShortcuts.Items.Add($"{shortcut.Key} → {shortcut.Value}");
                }
            }

            if (listBoxShortcuts.Items.Count == 1)
            {
                listBoxShortcuts.SelectedIndex = 0;
            }
        }

        private void ListBoxShortcuts_DoubleClick(object? sender, EventArgs e)
        {
            SelectCurrentItem();
        }

        private void ListBoxShortcuts_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectCurrentItem();
                e.Handled = true;
            }
        }

        private void SelectCurrentItem()
        {
            if (listBoxShortcuts.SelectedItem != null)
            {
                string selectedText = listBoxShortcuts.SelectedItem.ToString()!;
                SelectedShortcut = selectedText.Split('→')[0].Trim();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        public void NavigateList(int direction)
        {
            if (listBoxShortcuts.Items.Count == 0) return;

            int newIndex = listBoxShortcuts.SelectedIndex + direction;
            if (newIndex >= 0 && newIndex < listBoxShortcuts.Items.Count)
            {
                listBoxShortcuts.SelectedIndex = newIndex;
            }
        }

        public void UpdateFilter(string filter)
        {
            txtFilter.Text = filter ?? "";
            // Filtrowanie jest obsługiwane przez TxtFilter_TextChanged
        }
    }
} 