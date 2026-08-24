using System;
using Microsoft.Win32;
using USBUnlocker.Core;
using USBUnlocker.Utilities;

namespace USBUnlocker.USB
{
    public class UsbVerifier
    {
        private readonly AppConfig _config;
        
        public UsbVerifier(AppConfig config)
        {
            _config = config;
        }
        
        public UsbVerifyResult Verify()
        {
            var result = new UsbVerifyResult();
            
            // Check if USBSTOR is still blocked
            if (IsUsbStorBlocked())
                result.RemainingIssues++;
            
            // Check if write protection is still enabled
            if (IsWriteProtectionStillEnabled())
                result.RemainingIssues++;
            
            // Check if removable storage policies still exist
            if (HasRemovableStoragePolicy())
                result.RemainingIssues++;
            
            // Check device install restrictions
            if (HasDeviceInstallRestrictions())
                result.RemainingIssues++;
            
            result.AllBlocked = result.RemainingIssues > 0;
            
            return result;
        }
        
        public void VerifyAndDisplay()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     USB VERIFICATION");
            Console.WriteLine("============================================================\n");
            Console.WriteLine("Verifying USB access...\n");
            
            var result = Verify();
            
            Console.WriteLine("============================================================");
            Console.WriteLine("                     VERIFICATION RESULT");
            Console.WriteLine("============================================================\n");
            
            if (result.RemainingIssues == 0)
                Console.WriteLine("USB Status: AVAILABLE");
            else
                Console.WriteLine($"USB Status: BLOCKED ({result.RemainingIssues} issues remain)");
            
            Console.WriteLine("\n============================================================");
        }
        
        private bool IsUsbStorBlocked()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\USBSTOR"))
                {
                    if (key != null)
                    {
                        object value = key.GetValue("Start");
                        return value != null && (int)value == 4;
                    }
                }
            }
            catch { }
            return false;
        }
        
        private bool IsWriteProtectionStillEnabled()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\StorageDevicePolicies"))
                {
                    if (key != null)
                    {
                        object value = key.GetValue("WriteProtect");
                        return value != null && (int)value == 1;
                    }
                }
            }
            catch { }
            return false;
        }
        
        private bool HasRemovableStoragePolicy()
        {
            try
            {
                string[] values = { "Deny_Read", "Deny_Write", "Deny_Execute", "Deny_All" };
                foreach (string value in values)
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Policies\Microsoft\Windows\RemovableStorage"))
                    {
                        if (key != null)
                        {
                            object val = key.GetValue(value);
                            if (val != null && (int)val == 1)
                                return true;
                        }
                    }
                }
            }
            catch { }
            return false;
        }
        
        private bool HasDeviceInstallRestrictions()
        {
            try
            {
                string[] values = { "DenyRemovableDevices", "DenyUnspecified" };
                foreach (string value in values)
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Policies\Microsoft\Windows\DeviceInstall\Restrictions"))
                    {
                        if (key != null)
                        {
                            object val = key.GetValue(value);
                            if (val != null && (int)val == 1)
                                return true;
                        }
                    }
                }
            }
            catch { }
            return false;
        }
    }
}
