using System;
using System.IO;
using USBUnlocker.Core;

namespace USBUnlocker.Utilities
{
    public static class TempManager
    {
        public static void Cleanup(AppConfig config)
        {
            try
            {
                // Clean report temp
                if (Directory.Exists(config.ReportDir))
                {
                    try { Directory.Delete(config.ReportDir, true); }
                    catch { }
                }
                
                Logger.Info("Cleanup completed");
            }
            catch (Exception ex)
            {
                Logger.Warning("Cleanup error: " + ex.Message);
            }
        }
        
        public static void EnsureDirectories(AppConfig config)
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
