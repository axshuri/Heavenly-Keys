using System;
using USBUnlocker.Core;
using USBUnlocker.Utilities;

namespace USBUnlocker.USB
{
    public class UsbRepairWorkflow
    {
        private readonly AppConfig _config;
        
        public UsbRepairWorkflow(AppConfig config)
        {
            _config = config;
        }
        
        public void Execute()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     USB REPAIR");
            Console.WriteLine("============================================================\n");
            Console.WriteLine("This operation may modify local Windows USB policies.\n");
            Console.WriteLine("A backup will be created before any modification.\n");
            Console.WriteLine("Continue?\n");
            Console.WriteLine("[Y] Yes");
            Console.WriteLine("[N] No\n");
            Console.Write("Choice: ");
            
            string confirm = Console.ReadLine();
            if (confirm == null || confirm.ToUpper() != "Y")
                return;
            
            Console.WriteLine("\nScanning USB configuration...\n");
            
            // Scan
            var scanner = new UsbScanner(_config);
            var scanResult = scanner.Scan();
            
            // Repair
            Console.WriteLine("\nRepairing...\n");
            var repairer = new UsbRepairer(_config);
            repairer.Repair(scanResult);
            
            // Verify
            Console.WriteLine("\nVerifying...\n");
            var verifier = new UsbVerifier(_config);
            var verifyResult = verifier.Verify();
            
            // Display result
            Console.WriteLine("\n============================================================");
            Console.WriteLine("                 USB REPAIR COMPLETE");
            Console.WriteLine("============================================================\n");
            Console.WriteLine($"Detected restrictions:  {scanResult.RestrictionsDetected}");
            Console.WriteLine($"Repaired:              {scanResult.RestrictionsRepaired}");
            Console.WriteLine($"Remaining:             {verifyResult.RemainingIssues}\n");
            
            if (verifyResult.RemainingIssues == 0)
                Console.WriteLine("Final USB status: AVAILABLE");
            else
                Console.WriteLine("Final USB status: PARTIALLY REPAIRED");
            
            Console.WriteLine("\n============================================================");
        }
    }
}
