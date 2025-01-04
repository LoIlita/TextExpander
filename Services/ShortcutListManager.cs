using System.Windows.Forms;
using TextExpander.Interfaces;

namespace TextExpander.Services
{
    public class ShortcutListManager
    {
        private readonly ListView _listView;
        private readonly IShortcutManager _shortcutManager;

        public ShortcutListManager(ListView listView, IShortcutManager shortcutManager)
        {
            _listView = listView;
            _shortcutManager = shortcutManager;
        }

        public void RefreshList()
        {
            _listView.Items.Clear();
            foreach (var shortcut in _shortcutManager.GetAllShortcuts())
            {
                var item = new ListViewItem(shortcut.Key);
                item.SubItems.Add(shortcut.Value);
                _listView.Items.Add(item);
            }
        }

        public string? GetSelectedShortcut()
        {
            return _listView.SelectedItems.Count > 0 
                ? _listView.SelectedItems[0].Text 
                : null;
        }

        public void LoadShortcuts()
        {
            _shortcutManager.LoadShortcuts();
            RefreshList();
        }

        public bool AddShortcut(string key, string value)
        {
            if (_shortcutManager.AddShortcut(key, value))
            {
                _shortcutManager.SaveShortcuts();
                RefreshList();
                return true;
            }
            return false;
        }

        public bool UpdateShortcut(string key, string value)
        {
            if (_shortcutManager.UpdateShortcut(key, value))
            {
                _shortcutManager.SaveShortcuts();
                RefreshList();
                return true;
            }
            return false;
        }

        public bool DeleteShortcut(string key)
        {
            if (_shortcutManager.RemoveShortcut(key))
            {
                _shortcutManager.SaveShortcuts();
                RefreshList();
                return true;
            }
            return false;
        }
    }
} 