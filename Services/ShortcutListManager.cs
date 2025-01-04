using System.Windows.Forms;
using TextExpander.Interfaces;

namespace TextExpander.Services
{
    /// <summary>
    /// Klasa zarządzająca listą skrótów w interfejsie użytkownika.
    /// Odpowiada za wyświetlanie, aktualizację i zarządzanie listą skrótów w kontrolce ListView.
    /// </summary>
    public class ShortcutListManager
    {
        private readonly ListView _listView;
        private readonly IShortcutManager _shortcutManager;

        /// <summary>
        /// Inicjalizuje nową instancję klasy ShortcutListManager.
        /// </summary>
        /// <param name="listView">Kontrolka ListView do wyświetlania skrótów</param>
        /// <param name="shortcutManager">Menedżer skrótów dostarczający dane</param>
        public ShortcutListManager(ListView listView, IShortcutManager shortcutManager)
        {
            _listView = listView;
            _shortcutManager = shortcutManager;
            
            // Dodaj obsługę zmiany rozmiaru
            _listView.SizeChanged += (s, e) => ResizeColumns();
            
            // Dodaj obsługę zdarzeń myszy
            _listView.MouseMove += (s, e) => _listView.Invalidate();
            _listView.MouseLeave += (s, e) => _listView.Invalidate();
            
            // Dodaj obsługę podwójnego kliknięcia
            _listView.MouseDoubleClick += ListView_MouseDoubleClick;
        }

        /// <summary>
        /// Ładuje wszystkie skróty do kontrolki ListView.
        /// </summary>
        public void LoadShortcuts()
        {
            try
            {
                _listView.BeginUpdate();
                _listView.Items.Clear();
                foreach (var shortcut in _shortcutManager.GetAllShortcuts())
                {
                    var item = new ListViewItem(shortcut.Key);
                    item.SubItems.Add(shortcut.Value);
                    _listView.Items.Add(item);
                }
                
                if (_listView.Sorting != SortOrder.None)
                {
                    _listView.Sort();
                }
                
                ResizeColumns();
            }
            finally
            {
                _listView.EndUpdate();
            }
        }

        /// <summary>
        /// Dodaje nowy skrót do listy.
        /// </summary>
        /// <param name="key">Klucz skrótu</param>
        /// <param name="value">Wartość skrótu</param>
        /// <returns>True jeśli dodano pomyślnie, False jeśli skrót już istnieje</returns>
        public bool AddShortcut(string key, string value)
        {
            if (_shortcutManager.AddShortcut(key, value))
            {
                try
                {
                    _listView.BeginUpdate();
                    var item = new ListViewItem(key);
                    item.SubItems.Add(value);
                    _listView.Items.Add(item);
                    
                    if (_listView.Sorting != SortOrder.None)
                    {
                        _listView.Sort();
                    }
                    
                    ResizeColumns();
                }
                finally
                {
                    _listView.EndUpdate();
                }
                
                _shortcutManager.SaveShortcuts();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Aktualizuje istniejący skrót.
        /// </summary>
        /// <param name="oldKey">Klucz skrótu do zaktualizowania</param>
        /// <param name="newKey">Nowy klucz skrótu</param>
        /// <param name="newValue">Nowa wartość skrótu</param>
        public void UpdateShortcut(string oldKey, string newKey, string newValue)
        {
            try
            {
                _listView.BeginUpdate();
                
                // Usuń stary skrót
                _shortcutManager.DeleteShortcut(oldKey);
                
                // Dodaj nowy skrót z nowym kluczem i wartością
                _shortcutManager.AddShortcut(newKey, newValue);
                
                // Znajdź i zaktualizuj istniejący element
                foreach (ListViewItem item in _listView.Items)
                {
                    if (item.Text == oldKey)
                    {
                        item.Text = newKey;
                        item.SubItems[1].Text = newValue;
                        break;
                    }
                }
                
                _shortcutManager.SaveShortcuts();
            }
            finally
            {
                _listView.EndUpdate();
                _listView.Invalidate(true);
                _listView.Update();
            }
        }

        /// <summary>
        /// Usuwa skrót z listy.
        /// </summary>
        /// <param name="key">Klucz skrótu do usunięcia</param>
        public void DeleteShortcut(string key)
        {
            try
            {
                _listView.BeginUpdate();
                _shortcutManager.DeleteShortcut(key);
                
                // Znajdź i usuń konkretny element
                foreach (ListViewItem item in _listView.Items)
                {
                    if (item.Text == key)
                    {
                        _listView.Items.Remove(item);
                        break;
                    }
                }
                
                _shortcutManager.SaveShortcuts();
            }
            finally
            {
                _listView.EndUpdate();
                _listView.Invalidate(true);
                _listView.Update();
            }
        }

        /// <summary>
        /// Pobiera klucz aktualnie wybranego skrótu.
        /// </summary>
        /// <returns>Klucz wybranego skrótu lub null jeśli nic nie jest wybrane</returns>
        public string? GetSelectedShortcut()
        {
            return _listView.SelectedItems.Count > 0 
                ? _listView.SelectedItems[0].Text 
                : null;
        }

        private void ResizeColumns()
        {
            // Ustaw stałą szerokość dla kolumny ze skrótami
            const int shortcutColumnWidth = 100;
            _listView.Columns[0].Width = shortcutColumnWidth;
            
            // Druga kolumna zajmuje całą pozostałą przestrzeń plus 15 pikseli
            int remainingWidth = _listView.ClientSize.Width - shortcutColumnWidth - 4; // 4 piksele na marginesy
            _listView.Columns[1].Width = remainingWidth > 0 ? remainingWidth + 15 : 100;
        }

        private void ListView_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            var selectedKey = GetSelectedShortcut();
            if (selectedKey == null) return;

            try
            {
                var currentValue = _shortcutManager.GetShortcutValue(selectedKey);
                using (var form = new Forms.ShortcutForm(selectedKey, currentValue))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        UpdateShortcut(selectedKey, form.ShortcutKey, form.ShortcutValue);
                        _listView.Invalidate();
                        _listView.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Wystąpił błąd podczas edycji skrótu: {ex.Message}",
                    "Błąd",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
} 