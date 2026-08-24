using System;
using System.IO;

namespace USBUnlocker.Utilities
{
    public static class Logger
    {
        private static string _logFile;
        private static bool _debugMode;
        private static bool _initialized;
        
        public static void Initialize(string logFile, bool debugMode)
        {
            _logFile = logFile;
            _debugMode = debugMode;
            _initialized = true;
            
            try
            {
                string dir = Path.GetDirectoryName(logFile);
                if (!string.IsNullOrEmpty(dir))
                    Directory.CreateDirectory(dir);
            }
            catch { }
        }
        
        public static void Info(string message)
        {
            Log("INFO", message);
        }
        
        public static void Success(string message)
        {
            Log("SUCCESS", message);
        }
        
        public static void Warning(string message)
        {
            Log("WARNING", message);
        }
        
        public static void Error(string message)
        {
            Log("ERROR", message);
        }
        
        public static void Debug(string message)
        {
            if (_debugMode)
                Log("DEBUG", message);
        }
        
        private static void Log(string level, string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logEntry = $"[{timestamp}] [{level}] {message}";
            
            // Write to file
            if (_initialized && !string.IsNullOrEmpty(_logFile))
            {
                try
                {
                    File.AppendAllText(_logFile, logEntry + Environment.NewLine);
                }
                catch { }
            }
            
            // Write to console in debug mode
            if (_debugMode && level == "DEBUG")
            {
                Console.WriteLine(logEntry);
            }
        }
        
        public static void DisplayLog()
        {
            if (string.IsNullOrEmpty(_logFile) || !File.Exists(_logFile))
            {
                Console.WriteLine("No log file found.");
                return;
            }
            
            Console.WriteLine("============================================================");
            Console.WriteLine("                     REPAIR LOG");
            Console.WriteLine("============================================================\n");
            
            try
            {
                string content = File.ReadAllText(_logFile);
                Console.WriteLine(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading log file: " + ex.Message);
            }
            
            Console.WriteLine("\n============================================================");
        }
    }
}
