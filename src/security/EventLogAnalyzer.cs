using System;
using System.Diagnostics;
using USBUnlocker.Utilities;

namespace USBUnlocker.Security
{
    public class EventLogAnalyzer
    {
        public void Analyze()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     EVENT LOG ANALYSIS");
            Console.WriteLine("============================================================\n");
            Console.WriteLine("Analyzing recent USB and device events...\n");
            
            try
            {
                // System log
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("RECENT SYSTEM EVENTS (USB/Device related)");
                Console.WriteLine("------------------------------------------------------------");
                
                EventLog systemLog = new EventLog("System");
                int count = 0;
                
                foreach (EventLogEntry entry in systemLog.Entries)
                {
                    if (count >= 10) break;
                    
                    string source = entry.Source.ToLower();
                    if (source.Contains("usb") || source.Contains("pnp") || 
                        source.Contains("disk") || source.Contains("driver"))
                    {
                        Console.WriteLine($"\n  Time:     {entry.TimeGenerated}");
                        Console.WriteLine($"  Source:   {entry.Source}");
                        Console.WriteLine($"  Level:    {entry.EntryType}");
                        Console.WriteLine($"  Message:  {entry.Message?.Substring(0, Math.Min(100, entry.Message.Length))}...");
                        count++;
                    }
                }
                
                if (count == 0)
                    Console.WriteLine("  No recent USB/device events found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading event log: " + ex.Message);
            }
            
            Console.WriteLine("\n============================================================");
            Console.WriteLine("Event Log Analysis Complete");
            Console.WriteLine("============================================================");
        }
    }
}
