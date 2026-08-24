using System;
using System.Diagnostics;
using System.Security.Principal;

namespace USBUnlocker.Utilities
{
    public static class AdminHelper
    {
        public static bool IsAdministrator()
        {
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }
        
        public static bool TryElevate()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c start \"\" \"{AppDomain.CurrentDomain.BaseDirectory}\" /wait",
                    Verb = "runas",
                    UseShellExecute = true,
                    CreateNoWindow = false
                };
                
                Process process = Process.Start(startInfo);
                if (process != null)
                {
                    process.WaitForExit();
                    return true;
                }
                return false;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                // User declined UAC
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error("Elevation failed: " + ex.Message);
                return false;
            }
        }
    }
}
