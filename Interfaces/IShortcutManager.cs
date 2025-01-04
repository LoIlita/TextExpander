using System.Collections.Generic;

namespace TextExpander.Interfaces
{
    /// <summary>
    /// Interfejs definiujący operacje zarządzania skrótami w aplikacji.
    /// Umożliwia dodawanie, usuwanie, aktualizację i pobieranie skrótów oraz ich rozwinięć.
    /// </summary>
    public interface IShortcutManager
    {
        /// <summary>
        /// Pobiera wartość rozwinięcia dla podanego skrótu.
        /// </summary>
        /// <param name="shortcut">Skrót do wyszukania</param>
        /// <returns>Rozwinięcie skrótu lub null, jeśli skrót nie istnieje</returns>
        string? GetShortcutValue(string shortcut);

        /// <summary>
        /// Dodaje nowy skrót z jego rozwinięciem.
        /// </summary>
        /// <param name="shortcut">Skrót do dodania</param>
        /// <param name="value">Rozwinięcie skrótu</param>
        /// <returns>True jeśli dodano pomyślnie, False jeśli skrót już istnieje</returns>
        bool AddShortcut(string shortcut, string value);

        /// <summary>
        /// Usuwa skrót z systemu.
        /// </summary>
        /// <param name="shortcut">Skrót do usunięcia</param>
        void DeleteShortcut(string shortcut);

        /// <summary>
        /// Aktualizuje rozwinięcie dla istniejącego skrótu.
        /// </summary>
        /// <param name="shortcut">Skrót do zaktualizowania</param>
        /// <param name="newValue">Nowe rozwinięcie skrótu</param>
        void UpdateShortcut(string shortcut, string newValue);

        /// <summary>
        /// Pobiera wszystkie zdefiniowane skróty i ich rozwinięcia.
        /// </summary>
        /// <returns>Słownik zawierający pary skrót-rozwinięcie</returns>
        Dictionary<string, string> GetAllShortcuts();

        /// <summary>
        /// Zapisuje wszystkie skróty do pliku konfiguracyjnego.
        /// </summary>
        void SaveShortcuts();
    }
} 