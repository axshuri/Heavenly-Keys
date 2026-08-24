using System;
using System.Management;
using USBUnlocker.Utilities;

namespace USBUnlocker.Diagnostics
{
    public class HardwareInfoDisplay
    {
        public void Show()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     HARDWARE INFORMATION");
            Console.WriteLine("============================================================\n");
            
            // CPU
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("CPU");
            Console.WriteLine("------------------------------------------------------------");
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                foreach (ManagementObject cpu in searcher.Get())
                {
                    Console.WriteLine($"  Name:         {cpu["Name"]}");
                    Console.WriteLine($"  Cores:        {cpu["NumberOfCores"]}");
                    Console.WriteLine($"  Logical:      {cpu["NumberOfLogicalProcessors"]}");
                    Console.WriteLine($"  Max Clock:    {cpu["MaxClockSpeed"]} MHz");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error: " + ex.Message + "\n");
            }
            
            // Memory
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("MEMORY");
            Console.WriteLine("------------------------------------------------------------");
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject os in searcher.Get())
                {
                    long totalMem = Convert.ToInt64(os["TotalVisibleMemorySize"]) / 1024;
                    long freeMem = Convert.ToInt64(os["FreePhysicalMemory"]) / 1024;
                    Console.WriteLine($"  Total RAM:    {totalMem} MB");
                    Console.WriteLine($"  Free RAM:     {freeMem} MB");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error: " + ex.Message + "\n");
            }
            
            // GPU
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("GPU");
            Console.WriteLine("------------------------------------------------------------");
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
                foreach (ManagementObject gpu in searcher.Get())
                {
                    Console.WriteLine($"  Name:         {gpu["Name"]}");
                    Console.WriteLine($"  Driver:       {gpu["DriverVersion"]}");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error: " + ex.Message + "\n");
            }
            
            // Storage
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("STORAGE");
            Console.WriteLine("------------------------------------------------------------");
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject disk in searcher.Get())
                {
                    Console.WriteLine($"  Model:        {disk["Model"]}");
                    Console.WriteLine($"  Interface:    {disk["InterfaceType"]}");
                    Console.WriteLine($"  Size:         {FormatSize(disk["Size"])}");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error: " + ex.Message + "\n");
            }
            
            Console.WriteLine("============================================================");
        }
        
        private string FormatSize(object size)
        {
            if (size == null) return "N/A";
            try
            {
                long bytes = Convert.ToInt64(size);
                if (bytes >= 1073741824)
                    return $"{bytes / 1073741824.0:F1} GB";
                return $"{bytes / 1048576.0:F1} MB";
            }
            catch
            {
                return "N/A";
            }
        }
    }
}
