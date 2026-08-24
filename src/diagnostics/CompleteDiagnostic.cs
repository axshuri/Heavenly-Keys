using System;
using USBUnlocker.Core;
using USBUnlocker.USB;
using USBUnlocker.Network;
using USBUnlocker.Security;
using USBUnlocker.Reports;
using USBUnlocker.Utilities;

namespace USBUnlocker.Diagnostics
{
    public class CompleteDiagnostic
    {
        private readonly AppConfig _config;
        
        public CompleteDiagnostic(AppConfig config)
        {
            _config = config;
        }
        
        public void Execute()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     COMPLETE DIAGNOSTIC");
            Console.WriteLine("============================================================\n");
            Console.WriteLine("This will perform a full system diagnostic.\n");
            Console.WriteLine("Continue?\n");
            Console.WriteLine("[Y] Yes");
            Console.WriteLine("[N] No\n");
            Console.Write("Choice: ");
            
            string confirm = Console.ReadLine();
            if (confirm == null || confirm.ToUpper() != "Y")
                return;
            
            int step = 1;
            int total = 6;
            
            // System Info
            Console.WriteLine($"\n[{step}/{total}] System Information...");
            new SystemInfoDisplay().Show();
            step++;
            
            // USB Scan
            Console.WriteLine($"\n[{step}/{total}] USB Scan...");
            new UsbScanner(_config).ScanAndDisplay();
            step++;
            
            // Hardware
            Console.WriteLine($"\n[{step}/{total}] Hardware...");
            new HardwareInfoDisplay().Show();
            step++;
            
            // Network
            Console.WriteLine($"\n[{step}/{total}] Network...");
            new NetworkDiagnostic().Run();
            step++;
            
            // Security
            Console.WriteLine($"\n[{step}/{total}] Security...");
            new SecurityDiagnostic().Run();
            step++;
            
            // Reports
            Console.WriteLine($"\n[{step}/{total}] Reports...");
            new ReportGenerator(_config).GenerateAll();
            
            Console.WriteLine("\n============================================================");
            Console.WriteLine("COMPLETE DIAGNOSTIC FINISHED");
            Console.WriteLine("============================================================");
        }
    }
}
