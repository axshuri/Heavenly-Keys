using System;
using System.IO;
using Microsoft.Win32;
using USBUnlocker.Core;
using USBUnlocker.Utilities;

namespace USBUnlocker.USB
{
    public class RollbackManager
    {
        private readonly AppConfig _config;
        
        public RollbackManager(AppConfig config)
        {
            _config = config;
        }
        
        public void Rollback()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     ROLLBACK");
            Console.WriteLine("============================================================\n");
            
            // Find latest backup
            string backupDir = Path.Combine(_config.TempDir, "Backups");
            if (!Directory.Exists(backupDir))
            {
                Console.WriteLine("No backup found.");
                return;
            }
            
            string[] backups = Directory.GetDirectories(backupDir);
            if (backups.Length == 0)
            {
                Console.WriteLine("No backup found.");
                return;
            }
            
            Array.Sort(backups);
            string latestBackup = backups[backups.Length - 1];
            
            Console.WriteLine($"Last backup found: {Path.GetFileName(latestBackup)}\n");
            Console.WriteLine("Continue?\n");
            Console.WriteLine("[Y] Yes");
            Console.WriteLine("[N] No\n");
            Console.Write("Choice: ");
            
            string confirm = Console.ReadLine();
            if (confirm == null || confirm.ToUpper() != "Y")
                return;
            
            Console.WriteLine("\nRestoring configuration...");
            
            // Restore registry files
            RestoreRegistryFile(latestBackup, "USBSTOR.reg", @"HKLM\SYSTEM\CurrentControlSet\Services\USBSTOR");
            RestoreRegistryFile(latestBackup, "StorageDevicePolicies.reg", @"HKLM\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies");
            RestoreRegistryFile(latestBackup, "DeviceInstallRestrictions.reg", @"HKLM\Software\Policies\Microsoft\Windows\DeviceInstall\Restrictions");
            
            Console.WriteLine("\n============================================================");
            Console.WriteLine("ROLLBACK COMPLETED");
            Console.WriteLine("============================================================");
        }
        
        private void RestoreRegistryFile(string backupDir, string fileName, string keyPath)
        {
            string filePath = Path.Combine(backupDir, fileName);
            if (File.Exists(filePath))
            {
                try
                {
                    ProcessHelper.Run("reg.exe", $"import \"{filePath}\"");
                    Console.WriteLine($"  Restored: {fileName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Failed to restore {fileName}: {ex.Message}");
                }
            }
        }
    }
    
    // Simple process helper
    internal static class ProcessHelper
    {
        public static void Run(string fileName, string arguments)
        {
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            process.WaitForExit();
        }
    }
}
