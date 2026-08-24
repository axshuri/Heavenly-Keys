using System;
using Microsoft.Win32;
using USBUnlocker.Utilities;

namespace USBUnlocker.Security
{
    public class SecurityDiagnostic
    {
        public void Run()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     SECURITY DIAGNOSTIC");
            Console.WriteLine("============================================================\n");
            
            // Firewall
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("WINDOWS FIREWALL");
            Console.WriteLine("------------------------------------------------------------");
            CheckFirewall();
            Console.WriteLine();
            
            // UAC
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("UAC STATUS");
            Console.WriteLine("------------------------------------------------------------");
            CheckUAC();
            Console.WriteLine();
            
            // BitLocker
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("BITLOCKER");
            Console.WriteLine("------------------------------------------------------------");
            CheckBitLocker();
            Console.WriteLine();
            
            // Secure Boot
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("SECURE BOOT");
            Console.WriteLine("------------------------------------------------------------");
            CheckSecureBoot();
            
            Console.WriteLine("\n============================================================");
        }
        
        private void CheckFirewall()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\WindowsFirewall"))
                {
                    if (key != null)
                        Console.WriteLine("  Firewall policy detected");
                    else
                        Console.WriteLine("  Using default Windows Firewall");
                }
            }
            catch
            {
                Console.WriteLine("  Could not check firewall");
            }
        }
        
        private void CheckUAC()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"))
                {
                    if (key != null)
                    {
                        object enableLUA = key.GetValue("EnableLUA");
                        if (enableLUA != null)
                            Console.WriteLine($"  UAC: {((int)enableLUA == 1 ? "ENABLED" : "DISABLED")}");
                        else
                            Console.WriteLine("  UAC: Unknown");
                    }
                }
            }
            catch
            {
                Console.WriteLine("  Could not check UAC");
            }
        }
        
        private void CheckBitLocker()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\FVE"))
                {
                    if (key != null)
                    {
                        object denyWrite = key.GetValue("FDVDenyWriteAccess");
                        if (denyWrite != null && (int)denyWrite == 1)
                            Console.WriteLine("  BitLocker: Deny write access enabled");
                        else
                            Console.WriteLine("  BitLocker: No restrictions");
                    }
                    else
                    {
                        Console.WriteLine("  BitLocker: No policy restrictions");
                    }
                }
            }
            catch
            {
                Console.WriteLine("  Could not check BitLocker");
            }
        }
        
        private void CheckSecureBoot()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\SecureBoot\State"))
                {
                    if (key != null)
                    {
                        object enabled = key.GetValue("UEFISecureBootEnabled");
                        if (enabled != null)
                            Console.WriteLine($"  Secure Boot: {((int)enabled == 1 ? "ENABLED" : "DISABLED")}");
                        else
                            Console.WriteLine("  Secure Boot: Not available");
                    }
                    else
                    {
                        Console.WriteLine("  Secure Boot: Not supported");
                    }
                }
            }
            catch
            {
                Console.WriteLine("  Secure Boot: Not available");
            }
        }
    }
}
