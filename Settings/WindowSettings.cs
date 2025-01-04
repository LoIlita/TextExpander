using System;
using System.Drawing;
using System.IO;
using System.Text.Json;

namespace TextExpander.Settings
{
    /// <summary>
    /// Klasa przechowująca ustawienia okna aplikacji.
    /// </summary>
    public class WindowSettings
    {
        private const string SETTINGS_FILE = "window_settings.json";
        
        public Point Location { get; set; }
        public Size Size { get; set; }
        public bool IsMaximized { get; set; }

        public WindowSettings()
        {
            // Domyślne wartości
            Location = new Point(100, 100);
            Size = new Size(600, 440);
            IsMaximized = false;
        }

        /// <summary>
        /// Wczytuje ustawienia okna z pliku.
        /// </summary>
        public static WindowSettings Load()
        {
            try
            {
                if (File.Exists(SETTINGS_FILE))
                {
                    string json = File.ReadAllText(SETTINGS_FILE);
                    var settings = JsonSerializer.Deserialize<WindowSettings>(json);
                    return settings ?? new WindowSettings();
                }
            }
            catch (Exception)
            {
                // W przypadku błędu zwróć domyślne ustawienia
            }
            
            return new WindowSettings();
        }

        /// <summary>
        /// Zapisuje ustawienia okna do pliku.
        /// </summary>
        public void Save()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(this, options);
                File.WriteAllText(SETTINGS_FILE, json);
            }
            catch (Exception)
            {
                // Ignoruj błędy zapisu
            }
        }
    }
} 