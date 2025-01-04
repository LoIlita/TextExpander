using System.Collections.Generic;

namespace TextExpander.Interfaces
{
    public interface IShortcutManager
    {
        Dictionary<string, string> GetAllShortcuts();
        bool AddShortcut(string key, string value);
        bool RemoveShortcut(string key);
        bool UpdateShortcut(string key, string newValue);
        string GetShortcutValue(string key);
        void SaveShortcuts();
        void LoadShortcuts();
        string? GetExpansion(string shortcut);
    }
} 