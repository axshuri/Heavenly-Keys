using System;
using System.Management;
using USBUnlocker.Utilities;

namespace USBUnlocker.Diagnostics
{
    public class DriverAuditor
    {
        public void Audit()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     DRIVER AUDIT");
            Console.WriteLine("============================================================\n");
            Console.WriteLine("Scanning installed drivers...\n");
            
            int total = 0;
            int ok = 0;
            int problems = 0;
            
            try
            {
                // Count total devices
                var totalSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");
                foreach (ManagementObject device in totalSearcher.Get())
                    total++;
                
                // Find devices with problems
                var problemSearcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode > 0");
                
                Console.WriteLine("Devices with driver problems:\n");
                
                bool found = false;
                foreach (ManagementObject device in problemSearcher.Get())
                {
                    found = true;
                    problems++;
                    int errorCode = Convert.ToInt32(device["ConfigManagerErrorCode"]);
                    
                    Console.WriteLine($"  Name:  {device["Name"]}");
                    Console.WriteLine($"  Code:  {errorCode} ({GetErrorDescription(errorCode)})");
                    Console.WriteLine();
                }
                
                if (!found)
                {
                    Console.WriteLine("  No driver problems detected.\n");
                    ok = total;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error auditing drivers: " + ex.Message);
            }
            
            Console.WriteLine("============================================================");
            Console.WriteLine("Driver Audit Complete");
            Console.WriteLine("============================================================");
            Console.WriteLine($"Total devices: {total}");
            Console.WriteLine($"Problems:      {problems}");
            Console.WriteLine($"Status:        {(problems == 0 ? "OK" : "ISSUES FOUND")}");
            Console.WriteLine("============================================================");
        }
        
        private string GetErrorDescription(int code)
        {
            switch (code)
            {
                case 1: return "Device is not configured";
                case 3: return "Driver is corrupted";
                case 10: return "Device cannot start";
                case 12: return "Insufficient resources";
                case 14: return "Device cannot be cleaned";
                case 16: return "Lack of power";
                case 18: return "Device重启";
                case 22: return "Device disabled";
                case 24: return "Device not present";
                case 28: return "Driver not installed";
                case 31: return "Device not connected";
                case 32: return "Device not enabled";
                case 38: return "Driver cannot be loaded";
                case 39: return "Cannot initialize";
                case 43: return "Device is faulty";
                case 45: return "Device is offline";
                case 48: return "Driver signature violation";
                case 52: return "Device cannot be verified";
                default: return $"Error code {code}";
            }
        }
    }
}
