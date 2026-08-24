using System;
using USBUnlocker.Core;
using USBUnlocker.Utilities;

namespace USBUnlocker.USB
{
    public class DryRunner
    {
        private readonly AppConfig _config;
        
        public DryRunner(AppConfig config)
        {
            _config = config;
        }
        
        public void Run()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     DRY RUN MODE");
            Console.WriteLine("============================================================\n");
            Console.WriteLine("Performing detection only. NO SYSTEM CHANGES.\n");
            
            var scanner = new UsbScanner(_config);
            var result = scanner.Scan();
            
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("Potential repairs:");
            Console.WriteLine("------------------------------------------------------------\n");
            
            if (result.RestrictionsDetected > 0)
            {
                Console.WriteLine($"  Restrictions detected: {result.RestrictionsDetected}");
                Console.WriteLine("  These WOULD BE repaired in normal mode.\n");
            }
            else
            {
                Console.WriteLine("  No restrictions detected.\n");
            }
            
            Console.WriteLine("External restrictions:");
            Console.WriteLine("  Domain GPO       WOULD NOT MODIFY");
            Console.WriteLine("  Third-party DLP  WOULD NOT MODIFY");
            
            Console.WriteLine("\n============================================================");
            Console.WriteLine("NO SYSTEM CHANGES WERE MADE.");
            Console.WriteLine("============================================================");
        }
    }
}
