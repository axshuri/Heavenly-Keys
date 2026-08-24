using System;
using System.Management;
using USBUnlocker.Utilities;

namespace USBUnlocker.USB
{
    public class UsbDeviceEnumerator
    {
        public void Enumerate()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     USB DEVICE INFORMATION");
            Console.WriteLine("============================================================\n");
            
            try
            {
                // USB Controllers
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("USB CONTROLLERS");
                Console.WriteLine("------------------------------------------------------------");
                
                var controllerSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_USBController");
                foreach (ManagementObject controller in controllerSearcher.Get())
                {
                    Console.WriteLine($"  Name:   {controller["Name"]}");
                    Console.WriteLine($"  Status: {controller["Status"]}");
                    Console.WriteLine();
                }
                
                // USB Hubs
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("USB HUBS");
                Console.WriteLine("------------------------------------------------------------");
                
                var hubSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_USBHub");
                foreach (ManagementObject hub in hubSearcher.Get())
                {
                    Console.WriteLine($"  Name:   {hub["Name"]}");
                    Console.WriteLine($"  Status: {hub["Status"]}");
                    Console.WriteLine();
                }
                
                // USB Devices with problems
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("USB DEVICES WITH PROBLEMS");
                Console.WriteLine("------------------------------------------------------------");
                
                var problemSearcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode > 0 AND PNPClass = 'USB'");
                
                bool foundProblems = false;
                foreach (ManagementObject device in problemSearcher.Get())
                {
                    foundProblems = true;
                    Console.WriteLine($"  Name:   {device["Name"]}");
                    Console.WriteLine($"  Error:  Code {device["ConfigManagerErrorCode"]}");
                    Console.WriteLine();
                }
                
                if (!foundProblems)
                    Console.WriteLine("  No USB devices with problems detected.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error enumerating USB devices: " + ex.Message);
            }
            
            Console.WriteLine("============================================================");
        }
    }
}
