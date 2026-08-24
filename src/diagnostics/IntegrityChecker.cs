using System;
using System.Diagnostics;
using USBUnlocker.Utilities;

namespace USBUnlocker.Diagnostics
{
    public class IntegrityChecker
    {
        public void Check()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     WINDOWS INTEGRITY");
            Console.WriteLine("============================================================\n");
            Console.WriteLine("This operation may take several minutes.\n");
            
            Console.WriteLine("[SFC] System File Checker scan...");
            Console.WriteLine("Starting SFC /verifyonly (read-only)...\n");
            
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "sfc.exe",
                    Arguments = "/verifyonly",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                
                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit(30000); // 30 second timeout
                    
                    if (process.HasExited)
                    {
                        if (process.ExitCode == 0)
                            Console.WriteLine("SFC: System files OK");
                        else
                            Console.WriteLine("SFC: Issues detected or requires restart");
                    }
                    else
                    {
                        process.Kill();
                        Console.WriteLine("SFC: Timed out (30s limit)");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SFC: Could not run - " + ex.Message);
            }
            
            Console.WriteLine("\n============================================================");
            Console.WriteLine("Windows Integrity Check Complete");
            Console.WriteLine("============================================================");
        }
    }
}
