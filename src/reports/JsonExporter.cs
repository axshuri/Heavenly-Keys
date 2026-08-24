using System;
using System.IO;
using System.Text;
using USBUnlocker.Core;
using USBUnlocker.Utilities;

namespace USBUnlocker.Reports
{
    public class JsonExporter
    {
        private readonly AppConfig _config;
        
        public JsonExporter(AppConfig config)
        {
            _config = config;
        }
        
        public void Export()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     JSON EXPORT");
            Console.WriteLine("============================================================\n");
            Console.WriteLine("Generating JSON report...\n");
            
            try
            {
                Directory.CreateDirectory(_config.ReportDir);
                
                StringBuilder json = new StringBuilder();
                json.AppendLine("{");
                json.AppendLine("  \"application\": {");
                json.AppendLine("    \"name\": \"USBUnlocker\",");
                json.AppendLine("    \"version\": \"3.0.0\"");
                json.AppendLine("  },");
                json.AppendLine("  \"system\": {");
                json.AppendLine($"    \"computer\": \"{SystemInfo.ComputerName}\",");
                json.AppendLine($"    \"os\": \"{SystemInfo.OsVersion}\",");
                json.AppendLine($"    \"build\": \"{SystemInfo.OsBuild}\",");
                json.AppendLine($"    \"arch\": \"{SystemInfo.Architecture}\"");
                json.AppendLine("  },");
                json.AppendLine("  \"generated\": \"" + DateTime.Now.ToString("o") + "\"");
                json.AppendLine("}");
                
                string jsonFile = Path.Combine(_config.ReportDir, "Report.json");
                File.WriteAllText(jsonFile, json.ToString());
                
                Console.WriteLine($"JSON exported to:");
                Console.WriteLine($"  {jsonFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error exporting JSON: " + ex.Message);
            }
            
            Console.WriteLine("\n============================================================");
        }
    }
}
