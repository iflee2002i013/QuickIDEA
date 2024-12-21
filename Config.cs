using System;
using System.Text.Json;
using System.IO;

namespace InspirationRecorder
{
    public class Config
    {
        public string MarkdownFilePath { get; set; } = "D:\\Cursor_prj\\Windows\\QuickIDEA\\ideas.md";
        
        private static readonly string ConfigPath = "config.json";
        
        public static Config Load()
        {
            if (File.Exists(ConfigPath))
            {
                var json = File.ReadAllText(ConfigPath);
                return JsonSerializer.Deserialize<Config>(json) ?? new Config();
            }
            return new Config();
        }
        
        public void Save()
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigPath, json);
        }
    }
} 