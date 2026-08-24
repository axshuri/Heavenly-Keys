using System;
using System.Collections.Generic;
using Microsoft.Win32;
using USBUnlocker.Core;
using USBUnlocker.Utilities;

namespace USBUnlocker.USB
{
    public class UsbScanner
    {
        private readonly AppConfig _config;
        
        public UsbScanner(AppConfig config)
        {
            _config = config;
        }
        
        public UsbScanResult Scan()
        {
            var result = new UsbScanResult();
            
            // Check USBSTOR service
            if (IsUsbStorDisabled())
            {
                result.RestrictionsDetected++;
                Logger.Warning("USBSTOR: Service is disabled");
            }
            
            // Check write protection
            if (IsWriteProtectionEnabled())
            {
                result.RestrictionsDetected++;
                Logger.Warning("Write Protection: Enabled");
            }
            
            // Check removable storage policies
            if (HasRemovableStoragePolicy())
            {
                result.RestrictionsDetected++;
                Logger.Warning("Removable Storage: Policy detected");
            }
            
            // Check device installation restrictions
            if (HasDeviceInstallRestrictions())
            {
                result.RestrictionsDetected++;
                Logger.Warning("Device Installation: Restrictions detected");
            }
            
            // Check drive visibility
            if (HasDriveVisibilityPolicy())
            {
                result.RestrictionsDetected++;
                Logger.Warning("Drive Visibility: Policy detected");
            }
            
            // Check domain
            if (SystemInfo.DomainJoined)
            {
                result.DomainDetected = true;
                Logger.Info("Domain: Joined to " + SystemInfo.DomainName);
            }
            
            return result;
        }
        
        public void ScanAndDisplay()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     USB SCAN");
            Console.WriteLine("============================================================\n");
            Console.WriteLine("Scanning USB configuration...\n");
            
            var result = Scan();
            
            Console.WriteLine("============================================================");
            Console.WriteLine("                     SCAN COMPLETE");
            Console.WriteLine("============================================================\n");
            Console.WriteLine($"Detected restrictions:  {result.RestrictionsDetected}");
            Console.WriteLine($"Domain detected:        {result.DomainDetected}");
            Console.WriteLine();
            Console.WriteLine("============================================================");
        }
        
        private bool IsUsbStorDisabled()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\USBSTOR"))
                {
                    if (key != null)
                    {
                        object value = key.GetValue("Start");
                        if (value != null && (int)value == 4)
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("USBSTOR check error: " + ex.Message);
            }
            return false;
        }
        
        private bool IsWriteProtectionEnabled()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\StorageDevicePolicies"))
                {
                    if (key != null)
                    {
                        object value = key.GetValue("WriteProtect");
                        if (value != null && (int)value == 1)
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("WriteProtect check error: " + ex.Message);
            }
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
            catch (Exception ex)
            {
                Logger.Debug("RemovableStorage check error: " + ex.Message);
            }
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
            catch (Exception ex)
            {
                Logger.Debug("DeviceInstall check error: " + ex.Message);
            }
            return false;
        }
        
        private bool HasDriveVisibilityPolicy()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"))
                {
                    if (key != null)
                    {
                        object noDrives = key.GetValue("NoDrives");
                        object noView = key.GetValue("NoViewOnDrive");
                        
                        if (noDrives != null && (int)noDrives != 0)
                            return true;
                        if (noView != null && (int)noView != 0)
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("DriveVisibility check error: " + ex.Message);
            }
            return false;
        }
    }
}
