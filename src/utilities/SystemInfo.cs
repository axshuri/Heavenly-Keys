using System;
using System.Management;
using Microsoft.Win32;

namespace USBUnlocker.Utilities
{
    public static class SystemInfo
    {
        public static string OsVersion { get; private set; } = "Unknown";
        public static string OsBuild { get; private set; } = "0";
        public static string Architecture { get; private set; } = "x86";
        public static string ComputerName { get; private set; } = Environment.MachineName;
        public static string UserName { get; private set; } = Environment.UserName;
        public static bool DomainJoined { get; private set; }
        public static string DomainName { get; private set; } = "WORKGROUP";
        
        public static void Detect()
        {
            try
            {
                // Detect OS version
                using (ManagementObject os = new ManagementObject("Win32_OperatingSystem=@"))
                {
                    OsVersion = os["Caption"]?.ToString() ?? "Unknown";
                    OsBuild = os["BuildNumber"]?.ToString() ?? "0";
                }
                
                // Detect architecture
                Architecture = Environment.Is64BitOperatingSystem ? "x64" : "x86";
                
                // Detect domain
                using (ManagementObject cs = new ManagementObject("Win32_ComputerSystem=@"))
                {
                    DomainName = cs["Domain"]?.ToString() ?? "WORKGROUP";
                    DomainJoined = !DomainName.Contains("WORKGROUP");
                }
            }
            catch (Exception ex)
            {
                Logger.Warning("Could not detect system info: " + ex.Message);
                
                // Fallback detection
                try
                {
                    OsVersion = Environment.OSVersion.VersionString;
                    Architecture = Environment.Is64BitOperatingSystem ? "x64" : "x86";
                }
                catch { }
            }
        }
    }
}
