using System;
using Microsoft.Win32;
using USBUnlocker.Core;
using USBUnlocker.Utilities;

namespace USBUnlocker.USB
{
    public class UsbRepairer
    {
        private readonly AppConfig _config;
        
        public UsbRepairer(AppConfig config)
        {
            _config = config;
        }
        
        public void Repair(UsbScanResult scanResult)
        {
            Logger.Info("Starting USB repair...");
            
            // Repair USBSTOR
            RepairUsbStor();
            
            // Repair write protection
            RepairWriteProtection();
            
            // Repair removable storage policies
            RepairRemovableStorage();
            
            // Repair device installation restrictions
            RepairDeviceInstall();
            
            // Repair drive visibility
            RepairDriveVisibility();
            
            Logger.Success("USB repair completed");
        }
        
        private void RepairUsbStor()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\USBSTOR", true))
                {
                    if (key != null)
                    {
                        object value = key.GetValue("Start");
                        if (value != null && (int)value == 4)
                        {
                            key.SetValue("Start", 3, RegistryValueKind.DWord);
                            Logger.Success("USBSTOR: Restored to manual start");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("USBSTOR repair failed: " + ex.Message);
            }
        }
        
        private void RepairWriteProtection()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\StorageDevicePolicies", true))
                {
                    if (key != null)
                    {
                        object value = key.GetValue("WriteProtect");
                        if (value != null && (int)value == 1)
                        {
                            key.SetValue("WriteProtect", 0, RegistryValueKind.DWord);
                            Logger.Success("WriteProtection: Disabled");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("WriteProtection repair failed: " + ex.Message);
            }
        }
        
        private void RepairRemovableStorage()
        {
            try
            {
                string[] values = { "Deny_Read", "Deny_Write", "Deny_Execute", "Deny_All" };
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Policies\Microsoft\Windows\RemovableStorage", true))
                {
                    if (key != null)
                    {
                        foreach (string value in values)
                        {
                            object val = key.GetValue(value);
                            if (val != null && (int)val == 1)
                            {
                                key.DeleteValue(value, false);
                                Logger.Success($"RemovableStorage: Removed {value}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("RemovableStorage repair failed: " + ex.Message);
            }
        }
        
        private void RepairDeviceInstall()
        {
            try
            {
                string[] values = { "DenyRemovableDevices", "DenyUnspecified" };
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Policies\Microsoft\Windows\DeviceInstall\Restrictions", true))
                {
                    if (key != null)
                    {
                        foreach (string value in values)
                        {
                            object val = key.GetValue(value);
                            if (val != null && (int)val == 1)
                            {
                                key.DeleteValue(value, false);
                                Logger.Success($"DeviceInstall: Removed {value}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("DeviceInstall repair failed: " + ex.Message);
            }
        }
        
        private void RepairDriveVisibility()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", true))
                {
                    if (key != null)
                    {
                        object noDrives = key.GetValue("NoDrives");
                        if (noDrives != null && (int)noDrives != 0)
                        {
                            key.SetValue("NoDrives", 0, RegistryValueKind.DWord);
                            Logger.Success("NoDrives: Reset to 0");
                        }
                        
                        object noView = key.GetValue("NoViewOnDrive");
                        if (noView != null && (int)noView != 0)
                        {
                            key.SetValue("NoViewOnDrive", 0, RegistryValueKind.DWord);
                            Logger.Success("NoViewOnDrive: Reset to 0");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("DriveVisibility repair failed: " + ex.Message);
            }
        }
    }
}
