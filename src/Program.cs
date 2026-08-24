using System;
using System.Text;
using USBUnlocker.Core;
using USBUnlocker.UI;
using USBUnlocker.Utilities;

namespace USBUnlocker
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Enable UTF-8 output for Unicode art support
                Console.OutputEncoding = Encoding.UTF8;
                
                // Initialize configuration
                var config = AppConfig.Initialize(args);
                
                // Check for silent/batch mode
                if (config.SilentMode)
                {
                    RunSilentWorkflow(config);
                    return;
                }
                
                // Initialize application
                var app = new Application(config);
                app.Initialize();
                
                // Start main menu loop
                app.Run();
            }
            catch (Exception ex)
            {
                Logger.Error("Fatal error: " + ex.Message);
                Console.WriteLine("Fatal error occurred. Press any key to exit.");
                try { Console.ReadKey(); } catch { Console.ReadLine(); }
                Environment.Exit(5);
            }
        }
        
        static void RunSilentWorkflow(AppConfig config)
        {
            var app = new Application(config);
            app.Initialize();
            app.RunSilent();
        }
    }
}
