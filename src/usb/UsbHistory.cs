using System;
using Microsoft.Win32;
using USBUnlocker.Utilities;

namespace USBUnlocker.USB
{
    public class UsbHistory
    {
        public void Show()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     USB DEVICE HISTORY");
            Console.WriteLine("============================================================\n");
            Console.WriteLine("Scanning USB device history from registry...\n");
            
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum\USBSTOR"))
                {
                    if (key != null)
                    {
                        string[] subkeys = key.GetSubKeyNames();
                        Console.WriteLine($"Found {subkeys.Length} previously connected USB storage devices:\n");
                        
                        foreach (string subkey in subkeys)
                        {
                            Console.WriteLine($"  Device: {subkey}");
                            
                            using (RegistryKey deviceKey = key.OpenSubKey(subkey))
                            {
                                if (deviceKey != null)
                                {
                                    string[] deviceSubkeys = deviceKey.GetSubKeyNames();
                                    foreach (string serial in deviceSubkeys)
                                    {
                                        Console.WriteLine($"    Serial: {serial}");
                                        
                                        using (RegistryKey serialKey = deviceKey.OpenSubKey(serial))
                                        {
                                            if (serialKey != null)
                                            {
                                                object deviceDesc = serialKey.GetValue("DeviceDesc");
                                                if (deviceDesc != null)
                                                    Console.WriteLine($"    Description: {deviceDesc}");
                                                
                                                object mfg = serialKey.GetValue("Mfg");
                                                if (mfg != null)
                                                    Console.WriteLine($"    Manufacturer: {mfg}");
                                            }
                                        }
                                    }
                                }
                            }
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No USB device history found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading USB history: " + ex.Message);
            }
            
            Console.WriteLine("\n============================================================");
        }
    }
}
