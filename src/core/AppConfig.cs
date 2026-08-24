using System;
using System.IO;
using USBUnlocker.Utilities;

namespace USBUnlocker.Core
{
    public class AppConfig
    {
        public string ScriptDir { get; set; }
        public string ScriptPath { get; set; }
        public string Timestamp { get; set; }
        public string BackupDir { get; set; }
        public string LogDir { get; set; }
        public string LogFile { get; set; }
        public string TempDir { get; set; }
        public string ReportDir { get; set; }
        public string ZipFile { get; set; }
        
        public bool SilentMode { get; set; }
        public bool ScanOnly { get; set; }
        public bool RepairEnabled { get; set; }
        public bool RollbackMode { get; set; }
        public bool PortableMode { get; set; }
        public bool DebugMode { get; set; }
        
        public string ArtMode { get; set; } // "random" or "sequential"
        
        public static AppConfig Initialize(string[] args)
        {
            var config = new AppConfig();
            
            // Set paths
            config.ScriptPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            config.ScriptDir = Path.GetDirectoryName(config.ScriptPath) ?? ".";
            config.Timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
            
            // Determine data directory
            if (config.PortableMode || File.Exists(Path.Combine(config.ScriptDir, "portable.txt")))
            {
                config.TempDir = Path.Combine(config.ScriptDir, "Data");
            }
            else
            {
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                config.TempDir = Path.Combine(localAppData, "USBUnlocker");
            }
            
            // Create directories
            config.BackupDir = Path.Combine(config.TempDir, "Backups", config.Timestamp);
            config.LogDir = Path.Combine(config.TempDir, "Logs");
            config.ReportDir = Path.Combine(config.TempDir, "Reports", config.Timestamp);
            
            // Desktop ZIP
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            config.ZipFile = Path.Combine(desktop, $"USBUnlocker_Report_{config.Timestamp}.zip");
            
            // Log file
            config.LogFile = Path.Combine(config.LogDir, $"USBUnlocker_{config.Timestamp}.log");
            
            // Default settings
            config.SilentMode = false;
            config.ScanOnly = false;
            config.RepairEnabled = true;
            config.RollbackMode = false;
            config.PortableMode = false;
            config.DebugMode = false;
            config.ArtMode = "random";
            
            // Parse command line arguments
            ParseArgs(args, config);
            
            // Create directories
            EnsureDirectories(config);
            
            // Initialize logger
            Logger.Initialize(config.LogFile, config.DebugMode);
            
            return config;
        }
        
        private static void ParseArgs(string[] args, AppConfig config)
        {
            foreach (string arg in args)
            {
                switch (arg.ToLower())
                {
                    case "/silent":
                        config.SilentMode = true;
                        break;
                    case "/scan":
                        config.ScanOnly = true;
                        config.RepairEnabled = false;
                        break;
                    case "/repair":
                        config.RepairEnabled = true;
                        break;
                    case "/scanrepair":
                        config.RepairEnabled = true;
                        break;
                    case "/report":
                        config.ScanOnly = true;
                        break;
                    case "/rollback":
                        config.RollbackMode = true;
                        break;
                    case "/portable":
                        config.PortableMode = true;
                        break;
                    case "/debug":
                        config.DebugMode = true;
                        break;
                }
            }
        }
        
        private static void EnsureDirectories(AppConfig config)
        {
            try
            {
                Directory.CreateDirectory(config.TempDir);
                Directory.CreateDirectory(config.BackupDir);
                Directory.CreateDirectory(config.LogDir);
                Directory.CreateDirectory(config.ReportDir);
            }
            catch (Exception ex)
            {
                Logger.Warning("Could not create directories: " + ex.Message);
            }
        }
    }
}
