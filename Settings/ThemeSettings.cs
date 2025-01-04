using System.Drawing;
using System.Text.Json;
using System.IO;

namespace TextExpander.Settings
{
    /// <summary>
    /// Klasa przechowująca ustawienia motywu aplikacji.
    /// Implementuje wzorzec Singleton dla zapewnienia jednej instancji ustawień.
    /// </summary>
    public class ThemeSettings
    {
        private const string SETTINGS_FILE = "theme.json";
        private static ThemeSettings? _instance;
        private static readonly object _lock = new object();

        // Kolory dla ciemnego motywu
        private static readonly Color DARK_BACKGROUND = Color.FromArgb(30, 30, 30);      // #1E1E1E - Główne tło
        private static readonly Color DARK_LIST_BG1 = Color.FromArgb(42, 42, 42);        // #2A2A2A - Jasne wiersze
        private static readonly Color DARK_LIST_BG2 = Color.FromArgb(36, 36, 36);        // #242424 - Ciemne wiersze
        private static readonly Color DARK_HEADER = Color.FromArgb(238, 238, 238);       // #EEEEEE - Nagłówki
        private static readonly Color DARK_TEXT = Color.FromArgb(224, 224, 224);         // #E0E0E0 - Tekst
        private static readonly Color DARK_BORDER = Color.FromArgb(51, 51, 51);          // #333333 - Ramki
        private static readonly Color DARK_ACCENT = Color.FromArgb(3, 169, 244);         // #03A9F4 - Akcenty (niebieski)
        private static readonly Color DARK_BUTTON_BG = Color.FromArgb(45, 45, 45);       // #2D2D2D - Tło przycisków
        private static readonly Color DARK_BUTTON_HOVER = Color.FromArgb(60, 60, 60);    // #3C3C3C - Hover przycisków

        // Kolory dla jasnego motywu
        private static readonly Color LIGHT_BACKGROUND = Color.FromArgb(250, 250, 250);  // Jasne tło
        private static readonly Color LIGHT_FOREGROUND = Color.FromArgb(33, 33, 33);     // Ciemny tekst
        private static readonly Color LIGHT_CONTROL_BG = Color.FromArgb(255, 255, 255);  // Tło kontrolek
        private static readonly Color LIGHT_BUTTON_BG = Color.FromArgb(240, 240, 240);   // Tło przycisków
        private static readonly Color LIGHT_BUTTON_HOVER = Color.FromArgb(229, 241, 251); // Hover przycisków
        private static readonly Color LIGHT_BORDER = Color.FromArgb(213, 213, 213);      // Ramki

        /// <summary>
        /// Pobiera singleton instancję ustawień motywu.
        /// </summary>
        public static ThemeSettings Instance
        {
            get
            {
                lock (_lock)
                {
                    _instance ??= Load();
                    return _instance;
                }
            }
        }

        /// <summary>
        /// Określa, czy używany jest ciemny motyw.
        /// </summary>
        public bool IsDarkMode { get; set; }

        /// <summary>
        /// Pobiera kolor tła formularza na podstawie aktualnego motywu.
        /// </summary>
        public Color BackgroundColor => IsDarkMode ? DARK_BACKGROUND : LIGHT_BACKGROUND;

        /// <summary>
        /// Pobiera kolor tekstu na podstawie aktualnego motywu.
        /// </summary>
        public Color TextColor => IsDarkMode ? DARK_TEXT : LIGHT_FOREGROUND;

        /// <summary>
        /// Pobiera kolor nagłówków na podstawie aktualnego motywu.
        /// </summary>
        public Color HeaderColor => IsDarkMode ? DARK_HEADER : LIGHT_FOREGROUND;

        /// <summary>
        /// Pobiera kolor tła przycisków na podstawie aktualnego motywu.
        /// </summary>
        public Color ButtonBackgroundColor => IsDarkMode ? DARK_BUTTON_BG : LIGHT_BUTTON_BG;

        /// <summary>
        /// Pobiera kolor obramowania na podstawie aktualnego motywu.
        /// </summary>
        public Color BorderColor => IsDarkMode ? DARK_BORDER : LIGHT_BORDER;

        /// <summary>
        /// Pobiera kolor akcentu na podstawie aktualnego motywu.
        /// </summary>
        public Color AccentColor => IsDarkMode ? DARK_ACCENT : Color.FromArgb(0, 120, 215);

        /// <summary>
        /// Pobiera pierwszy kolor tła listy (jasne wiersze).
        /// </summary>
        public Color ListBackgroundColor1 => IsDarkMode ? DARK_LIST_BG1 : LIGHT_CONTROL_BG;

        /// <summary>
        /// Pobiera drugi kolor tła listy (ciemne wiersze).
        /// </summary>
        public Color ListBackgroundColor2 => IsDarkMode ? DARK_LIST_BG2 : Color.FromArgb(248, 248, 248);

        private ThemeSettings()
        {
            IsDarkMode = false;
        }

        /// <summary>
        /// Zapisuje ustawienia motywu do pliku.
        /// </summary>
        public void Save()
        {
            var json = JsonSerializer.Serialize(this);
            File.WriteAllText(SETTINGS_FILE, json);
        }

        /// <summary>
        /// Wczytuje ustawienia motywu z pliku lub tworzy nowe z domyślnymi wartościami.
        /// </summary>
        private static ThemeSettings Load()
        {
            if (File.Exists(SETTINGS_FILE))
            {
                try
                {
                    var json = File.ReadAllText(SETTINGS_FILE);
                    var settings = JsonSerializer.Deserialize<ThemeSettings>(json);
                    return settings ?? new ThemeSettings();
                }
                catch
                {
                    return new ThemeSettings();
                }
            }
            return new ThemeSettings();
        }
    }
} 