using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TextExpander.Interfaces;

namespace TextExpander.Services
{
    /// <summary>
    /// Implementacja menedżera skrótów wykorzystująca format JSON do przechowywania danych.
    /// Zarządza zapisem i odczytem skrótów z pliku JSON oraz operacjami na skrótach.
    /// </summary>
    public class JsonShortcutManager : IShortcutManager
    {
        private readonly string _filePath;
        private readonly Dictionary<string, string> _shortcuts;

        /// <summary>
        /// Inicjalizuje nową instancję klasy JsonShortcutManager.
        /// </summary>
        /// <param name="filePath">Ścieżka do pliku JSON ze skrótami</param>
        public JsonShortcutManager(string filePath)
        {
            _filePath = filePath;
            _shortcuts = LoadShortcutsFromFile();
        }

        /// <summary>
        /// Pobiera wartość rozwinięcia dla podanego skrótu.
        /// </summary>
        /// <param name="shortcut">Skrót do wyszukania</param>
        /// <returns>Rozwinięcie skrótu lub null, jeśli skrót nie istnieje</returns>
        public string? GetShortcutValue(string shortcut)
        {
            return _shortcuts.TryGetValue(shortcut, out string? value) ? value : null;
        }

        /// <summary>
        /// Dodaje nowy skrót z jego rozwinięciem.
        /// </summary>
        /// <param name="shortcut">Skrót do dodania</param>
        /// <param name="value">Rozwinięcie skrótu</param>
        /// <returns>True jeśli dodano pomyślnie, False jeśli skrót już istnieje</returns>
        public bool AddShortcut(string shortcut, string value)
        {
            if (_shortcuts.ContainsKey(shortcut))
                return false;

            _shortcuts[shortcut] = value;
            return true;
        }

        /// <summary>
        /// Usuwa skrót z systemu.
        /// </summary>
        /// <param name="shortcut">Skrót do usunięcia</param>
        public void DeleteShortcut(string shortcut)
        {
            _shortcuts.Remove(shortcut);
        }

        /// <summary>
        /// Aktualizuje rozwinięcie dla istniejącego skrótu.
        /// </summary>
        /// <param name="shortcut">Skrót do zaktualizowania</param>
        /// <param name="newValue">Nowe rozwinięcie skrótu</param>
        public void UpdateShortcut(string shortcut, string newValue)
        {
            if (_shortcuts.ContainsKey(shortcut))
            {
                _shortcuts[shortcut] = newValue;
                SaveShortcuts();
            }
        }

        /// <summary>
        /// Pobiera wszystkie zdefiniowane skróty i ich rozwinięcia.
        /// </summary>
        /// <returns>Słownik zawierający pary skrót-rozwinięcie</returns>
        public Dictionary<string, string> GetAllShortcuts()
        {
            return new Dictionary<string, string>(_shortcuts);
        }

        /// <summary>
        /// Zapisuje wszystkie skróty do pliku JSON.
        /// </summary>
        public void SaveShortcuts()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(_shortcuts, options);
            File.WriteAllText(_filePath, jsonString);
        }

        /// <summary>
        /// Wczytuje skróty z pliku JSON.
        /// </summary>
        /// <returns>Słownik zawierający wczytane skróty i ich rozwinięcia</returns>
        private Dictionary<string, string> LoadShortcutsFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new Dictionary<string, string>();
            }

            try
            {
                string jsonString = File.ReadAllText(_filePath);
                var shortcuts = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
                return shortcuts ?? new Dictionary<string, string>();
            }
            catch (Exception)
            {
                return new Dictionary<string, string>();
            }
        }
    }
} 