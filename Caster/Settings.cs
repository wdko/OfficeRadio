using System;
using System.IO;
using System.Text.Json;

namespace Caster
{
    public class Settings
    {
        public bool MinimizeToTray { get; set; } = false;
        public bool ExitToTray { get; set; } = false;
        public string? FavoriteDeviceName { get; set; } = null;

        private static readonly string SettingsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "OfficeRadio");
        private static readonly string SettingsFilePath = Path.Combine(SettingsDirectory, "settings.json");

        public static Settings Load()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    var json = File.ReadAllText(SettingsFilePath);
                    return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
                }
            }
            catch (Exception ex)
            {
                // Log error if needed, but continue with default settings
                System.Diagnostics.Debug.WriteLine($"Error loading settings: {ex.Message}");
            }
            return new Settings();
        }

        public void Save()
        {
            try
            {
                Directory.CreateDirectory(SettingsDirectory);
                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                // Log error if needed
                System.Diagnostics.Debug.WriteLine($"Error saving settings: {ex.Message}");
            }
        }
    }
}