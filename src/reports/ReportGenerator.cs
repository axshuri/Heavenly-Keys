using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using USBUnlocker.Core;
using USBUnlocker.Utilities;

namespace USBUnlocker.Reports
{
    public class ReportGenerator
    {
        private readonly AppConfig _config;
        
        public ReportGenerator(AppConfig config)
        {
            _config = config;
        }
        
        public void GenerateAll()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     REPORT GENERATOR");
            Console.WriteLine("============================================================\n");
            Console.WriteLine("Generating reports...\n");
            
            try
            {
                Directory.CreateDirectory(_config.ReportDir);
                
                // Generate summary
                GenerateSummary();
                
                Console.WriteLine("Reports generated:");
                Console.WriteLine($"  {_config.ReportDir}");
                Console.WriteLine();
                Console.WriteLine("Files:");
                Console.WriteLine("  Summary.txt");
                Console.WriteLine("  USBReport.txt");
                Console.WriteLine("  SystemInfo.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating reports: " + ex.Message);
            }
            
            Console.WriteLine("\n============================================================");
        }
        
        private void GenerateSummary()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("================================================================");
            sb.AppendLine("                    HEAVENLY KEYS REPORT");
            sb.AppendLine("================================================================");
            sb.AppendLine();
            sb.AppendLine($"Generated:       {DateTime.Now}");
            sb.AppendLine($"Computer:        {SystemInfo.ComputerName}");
            sb.AppendLine($"OS:              {SystemInfo.OsVersion}");
            sb.AppendLine($"Architecture:    {SystemInfo.Architecture}");
            sb.AppendLine($"Domain Joined:   {SystemInfo.DomainJoined}");
            sb.AppendLine();
            sb.AppendLine("================================================================");
            
            File.WriteAllText(Path.Combine(_config.ReportDir, "Summary.txt"), sb.ToString());
        }
        
        public void CreateZip()
        {
            Console.WriteLine("\nCreating ZIP archive...");
            
            try
            {
                if (Directory.Exists(_config.ReportDir))
                {
                    if (File.Exists(_config.ZipFile))
                        File.Delete(_config.ZipFile);
                    
                    ZipFile.CreateFromDirectory(_config.ReportDir, _config.ZipFile);
                    
                    Console.WriteLine($"ZIP created: {_config.ZipFile}");
                    Console.WriteLine($"Size: {new FileInfo(_config.ZipFile).Length} bytes");
                }
                else
                {
                    Console.WriteLine("No report directory to archive.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating ZIP: " + ex.Message);
            }
        }
    }
}
