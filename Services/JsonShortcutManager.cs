using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TextExpander.Interfaces;

namespace TextExpander.Services
{
    public class JsonShortcutManager : IShortcutManager
    {
        private Dictionary<string, string> _shortcuts;
        private readonly string _filePath;

        public JsonShortcutManager(string filePath)
        {
            _filePath = filePath;
            _shortcuts = new Dictionary<string, string>();
        }

        public Dictionary<string, string> GetAllShortcuts()
        {
            return new Dictionary<string, string>(_shortcuts);
        }

        public bool AddShortcut(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
                return false;

            if (_shortcuts.ContainsKey(key))
                return false;

            _shortcuts.Add(key, value);
            return true;
        }

        public bool RemoveShortcut(string key)
        {
            return _shortcuts.Remove(key);
        }

        public bool UpdateShortcut(string key, string newValue)
        {
            if (!_shortcuts.ContainsKey(key))
                return false;

            _shortcuts[key] = newValue;
            return true;
        }

        public string GetShortcutValue(string key)
        {
            return _shortcuts.TryGetValue(key, out string value) ? value : string.Empty;
        }

        public void SaveShortcuts()
        {
            try
            {
                string json = JsonSerializer.Serialize(_shortcuts, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                // W prawdziwej aplikacji należałoby dodać odpowiednie logowanie błędów
                System.Diagnostics.Debug.WriteLine($"Error saving shortcuts: {ex.Message}");
            }
        }

        public void LoadShortcuts()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    _shortcuts = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
                }
                else
                {
                    // Domyślne skróty
                    _shortcuts = new Dictionary<string, string>
                    {
                        { "@mail", "example@email.com" },
                        { "@tel", "+48 123 456 789" },
                        { "@lorem", "Lorem ipsum dolor sit amet" }
                    };
                    SaveShortcuts();
                }
            }
            catch (Exception ex)
            {
                // W prawdziwej aplikacji należałoby dodać odpowiednie logowanie błędów
                System.Diagnostics.Debug.WriteLine($"Error loading shortcuts: {ex.Message}");
                _shortcuts = new Dictionary<string, string>();
            }
        }

        public string? GetExpansion(string shortcut)
        {
            if (_shortcuts.TryGetValue(shortcut, out string? value))
            {
                return value;
            }
            return null;
        }
    }
} 