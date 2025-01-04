using System;
using System.Text.Json;
using System.Windows.Forms;

namespace TextExpander.Settings
{
    public class AppSettings
    {
        private const string SETTINGS_FILE = "settings.json";
        public Keys TriggerKey { get; set; } = Keys.M;
        public bool RequireControl { get; set; } = true;
        public bool RequireAlt { get; set; } = false;
        public bool RequireShift { get; set; } = false;

        public static AppSettings Load()
        {
            try
            {
                if (System.IO.File.Exists(SETTINGS_FILE))
                {
                    string json = System.IO.File.ReadAllText(SETTINGS_FILE);
                    return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading settings: {ex.Message}");
            }
            return new AppSettings();
        }

        public void Save()
        {
            try
            {
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(SETTINGS_FILE, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        public bool IsTriggered(KeyEventArgs e)
        {
            return e.KeyCode == TriggerKey &&
                   e.Control == RequireControl &&
                   e.Alt == RequireAlt &&
                   e.Shift == RequireShift;
        }

        public string GetKeyDescription()
        {
            string description = "";
            if (RequireControl) description += "Ctrl + ";
            if (RequireAlt) description += "Alt + ";
            if (RequireShift) description += "Shift + ";
            description += TriggerKey.ToString();
            return description;
        }
    }
} 