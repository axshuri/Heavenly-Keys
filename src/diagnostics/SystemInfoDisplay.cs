using System;
using System.Management;
using USBUnlocker.Utilities;

namespace USBUnlocker.Diagnostics
{
    public class SystemInfoDisplay
    {
        public void Show()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     SYSTEM INFORMATION");
            Console.WriteLine("============================================================\n");
            
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("OPERATING SYSTEM");
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine($"  OS Version:     {SystemInfo.OsVersion}");
            Console.WriteLine($"  Build:          {SystemInfo.OsBuild}");
            Console.WriteLine($"  Architecture:   {SystemInfo.Architecture}");
            Console.WriteLine($"  Computer:       {SystemInfo.ComputerName}");
            Console.WriteLine($"  User:           {SystemInfo.UserName}");
            Console.WriteLine();
            
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("NETWORK MEMBERSHIP");
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine($"  Domain Joined:  {SystemInfo.DomainJoined}");
            Console.WriteLine($"  Domain/Group:   {SystemInfo.DomainName}");
            Console.WriteLine();
            
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("DRIVES");
            Console.WriteLine("------------------------------------------------------------");
            
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk WHERE DriveType = 3 OR DriveType = 2");
                foreach (ManagementObject disk in searcher.Get())
                {
                    Console.WriteLine($"  {disk["DeviceID"]}  {disk["FileSystem"]}  " +
                        $"{FormatSize(disk["Size"])} total  {FormatSize(disk["FreeSpace"])} free");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error reading drives: " + ex.Message);
            }
            
            Console.WriteLine("\n============================================================");
        }
        
        private string FormatSize(object size)
        {
            if (size == null) return "N/A";
            try
            {
                long bytes = Convert.ToInt64(size);
                if (bytes >= 1073741824)
                    return $"{bytes / 1073741824.0:F1} GB";
                if (bytes >= 1048576)
                    return $"{bytes / 1048576.0:F1} MB";
                return $"{bytes / 1024.0:F1} KB";
            }
            catch
            {
                return "N/A";
            }
        }
    }
}
