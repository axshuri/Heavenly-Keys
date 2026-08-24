using System;

namespace USBUnlocker.Utilities
{
    public static class DisplayHelper
    {
        public static void ClearScreen()
        {
            try
            {
                Console.Clear();
            }
            catch
            {
                // Console.Clear() can fail in some environments
                Console.WriteLine(new string('\n', 50));
            }
        }
        
        public static void PauseReturnToMenu()
        {
            Console.WriteLine("\nPress any key to return to the Main Menu...");
            try
            {
                Console.ReadKey(true);
            }
            catch
            {
                Console.ReadLine();
            }
        }
        
        public static void ShowElevationRequest()
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("                   HEAVENLY KEYS");
            Console.WriteLine("============================================================");
            Console.WriteLine();
            Console.WriteLine("Administrator privileges are required.");
            Console.WriteLine();
            Console.WriteLine("Requesting Administrator access...");
            Console.WriteLine();
            Console.WriteLine("Please approve the Windows UAC prompt.");
            Console.WriteLine("============================================================");
        }
        
        public static void ShowAccessDenied()
        {
            Console.WriteLine();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     ACCESS DENIED");
            Console.WriteLine("============================================================");
            Console.WriteLine();
            Console.WriteLine("USBUnlocker requires Administrator privileges.");
            Console.WriteLine();
            Console.WriteLine("Some diagnostic information may still be available,");
            Console.WriteLine("but system repair cannot continue.");
            Console.WriteLine();
            Console.WriteLine("Please run this program as Administrator.");
            Console.WriteLine("============================================================");
        }
        
        public static void ShowFinalSummary(UsbScanResult scan, UsbVerifyResult verify)
        {
            Console.WriteLine();
            Console.WriteLine("============================================================");
            Console.WriteLine("                    FINAL USB REPORT");
            Console.WriteLine("============================================================");
            Console.WriteLine();
            Console.WriteLine($"Detected restrictions:  {scan.RestrictionsDetected}");
            Console.WriteLine($"Repaired:              {scan.RestrictionsRepaired}");
            Console.WriteLine($"Unresolved:            {verify.RemainingIssues}");
            Console.WriteLine();
            
            if (verify.RemainingIssues == 0)
                Console.WriteLine("Final USB status: AVAILABLE");
            else
                Console.WriteLine("Final USB status: PARTIALLY REPAIRED");
            
            Console.WriteLine();
            Console.WriteLine("============================================================");
        }
    }
    
    // Simple result types for USB operations
    public class UsbScanResult
    {
        public int RestrictionsDetected { get; set; }
        public int RestrictionsRepaired { get; set; }
        public bool DomainDetected { get; set; }
        public bool ExternalDetected { get; set; }
    }
    
    public class UsbVerifyResult
    {
        public int RemainingIssues { get; set; }
        public bool AllBlocked { get; set; }
    }
}
