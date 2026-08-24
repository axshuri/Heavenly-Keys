using System;
using System.ServiceProcess;
using USBUnlocker.Utilities;

namespace USBUnlocker.Diagnostics
{
    public class ServiceAuditor
    {
        private readonly string[] _criticalServices = new string[]
        {
            "USBSTOR", "PlugPlay", "RpcSs", "Winmgmt", 
            "DeviceInstall", "Dhcp", "Dnscache", "WSearch"
        };
        
        public void Audit()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     SERVICE AUDIT");
            Console.WriteLine("============================================================\n");
            Console.WriteLine("Auditing critical Windows services...\n");
            
            foreach (string serviceName in _criticalServices)
            {
                try
                {
                    ServiceController service = new ServiceController(serviceName);
                    ServiceControllerStatus status = service.Status;
                    Console.WriteLine($"  {serviceName,-20} {status}");
                }
                catch (Exception)
                {
                    Console.WriteLine($"  {serviceName,-20} NOT FOUND");
                }
            }
            
            Console.WriteLine("\n============================================================");
            Console.WriteLine("Service Audit Complete");
            Console.WriteLine("============================================================");
        }
    }
}
