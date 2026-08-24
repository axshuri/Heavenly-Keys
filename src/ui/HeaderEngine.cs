using System;
using System.Threading;
using USBUnlocker.Core;
using USBUnlocker.Utilities;

namespace USBUnlocker.UI
{
    public class HeaderEngine
    {
        private static readonly string[] LoadingMessages = {
            "Initializing Heavenly Keys v3.0...",
            "Loading system diagnostics...",
            "Detecting USB controllers...",
            "Scanning driver inventory...",
            "Preparing repair modules...",
            "Building capability registry...",
            "Ready."
        };
        
        private readonly AppConfig _config;
        
        public HeaderEngine() { }
        
        public HeaderEngine(AppConfig config)
        {
            _config = config;
        }
        
        public void LoadArtwork()
        {
            // No-op: ASCII art removed, using loading screen instead
        }
        
        public void ShowLoadingScreen()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                   HEAVENLY KEYS");
            Console.WriteLine("============================================================");
            Console.WriteLine();
            Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⠀⡤⢤⣀⣤⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⢀⣠⢶⠞⢩⣧⡨⠿⠿⢿⡝⠯⠛⠶⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⠀⠀⢀⣶⠟⠍⠁⢒⠿⡠⠖⠉⠉⢙⣷⠀⠀⠀⠈⠩⣲⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⠀⢤⡿⣥⡖⣲⣿⣿⣞⣁⣀⠴⢚⣿⠛⣷⡈⣆⠀⠱⡌⠉⢧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⢰⡿⢛⣶⣿⣿⣿⠋⣹⣟⣁⣴⣾⠃⢀⡏⠇⠸⡀⠀⢱⠀⢈⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⣿⡇⡘⣾⣿⣿⡇⣸⡯⠽⠟⢋⣉⠑⡞⠀⡼⢠⢧⠀⠀⡇⠈⢿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠐⡿⢰⢁⡟⠀⠉⣰⠙⡿⣷⣶⢦⡄⢰⠁⢰⠃⣸⡌⠀⢸⠃⢀⢾⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⣷⢸⢸⢧⡰⢼⣿⡀⠉⠀⠈⠀⠀⠀⢧⢇⣸⣳⠁⡰⢃⠀⣸⣿⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⢿⣿⡸⣼⡝⢦⠣⠁⠀⠀⠀⠀⠀⠀⠘⠙⠻⢥⠞⢁⠜⣰⣿⣿⡿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⠈⢿⢿⣼⣇⠘⣧⡀⠀⠀⠀⠀⠀⠄⠀⠀⠀⠀⣼⣧⣾⡷⠛⢿⠓⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⠀⠸⠺⣿⣿⣇⣿⠙⢦⡀⠀⠀⠀⠀⠀⠀⢀⣼⡿⠋⠀⠀⠀⠈⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⢀⡤⠶⠶⠿⢿⣿⡇⠀⠀⠈⠓⠤⣤⡤⠖⠊⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⡴⠋⠀⠀⠀⠀⠀⠙⠓⠤⠄⣀⡀⠀⢸⣷⣦⡤⠤⠖⠒⠒⠢⢤⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⢸⠃⠀⠀⠀⠀⠀⢀⢆⡀⠀⠂⠒⠒⠒⠻⠦⣄⡀⠀⢀⠢⠤⠤⢄⡹⣦⣀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⡚⠀⠀⠀⠀⠀⠀⡸⠋⠀⠀⠀⠀⠀⢀⠀⠀⠀⠈⠳⡄⠀⠀⠀⠀⠀⠈⠉⠳⣤⡀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⢹⠀⠀⠀⠀⠀⢠⠇⠀⠀⠀⠀⠀⠀⠻⠇⠀⠀⠀⠀⠙⡄⠀⠀⠀⠀⠀⠀⠀⢬⣱⣄⠀⠀⠀⠀");
            Console.WriteLine("⠀⣇⠀⠀⠀⠀⢸⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⠀⠀⠀⠀");
            Console.WriteLine("⠀⠻⡄⠀⠀⠀⠀⢇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⡿⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⣷⠀⠀⠀⠀⠈⡦⣀⠀⠀⠀⠀⠀⠀⠀⣀⠠⠖⠋⠈⠳⣄⠀⠀⠀⠀⠀⠀⢠⡟⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠹⡄⠀⠀⠀⠀⢸⠈⠉⠒⠒⠒⠊⠉⠁⠀⠀⠀⠀⠀⠀⠈⠳⣆⡀⠀⢀⡴⠟⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⢥⠀⠀⠀⠀⢸⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠐⡿⠛⠉⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⢸⡄⠀⠀⠀⢸⣧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⠀⠀⣧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⠈⣷⠀⠀⠀⠀⡹⣿⡴⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠀⠀⠛⢧⡀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⠀⣿⠄⠀⠀⠀⣿⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠱⣄⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⠀⠭⠄⠀⠀⡰⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠻⡄⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⠀⠸⡆⠀⢰⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠓⢄⠀⠀");
            Console.WriteLine("⠀⠀⠀⠀⠀⢻⣄⡎⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠢⡱⡄");
            Console.WriteLine("⠀⠀⠀⠀⠀⠈⡿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⠀⠀⠁⠙⡄");
            Console.WriteLine("⠀⠀⠀⠀⠀⢸⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⠃⠀⠀⠀⠀⠹⡄");
            Console.WriteLine("⠀⠀⠀⠀⢀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠑⢦⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⠏⠀⠀⠀⠀⠀⠀⠹");
            Console.WriteLine("⠀⠀⠀⠀⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠢⡀⠀⠀⠀⠀⠀⠀⠀⡞⠀⠀⠀⠀⠀⠀⠀⠀⢳");
            Console.WriteLine("⠀⠀⠀⠀⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠢⡀⠀⠀⠀⠀⢀⡇⠀⠀⠀⠀⠀⠀⠀⠀⢸");
            Console.WriteLine("⠀⠀⠀⠀⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠹⡄⠀⠀⠀⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈");
            Console.WriteLine("⠀⠀⠀⠀⠘⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⡆⢀⣾⠏⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⠀⠀⢳⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢹⡏⠉⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine("⠀⠀⠀⠀⠀⠈⢧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢳⡀⠀⠀⠀⠀⠀⠀⠀⠀");
            Console.WriteLine();
            Console.WriteLine("============================================================");
            Console.WriteLine();
            
            int totalSeconds = 10;
            int msgIndex = 0;
            int msgInterval = totalSeconds / LoadingMessages.Length;
            if (msgInterval < 1) msgInterval = 1;
            
            for (int i = 0; i < totalSeconds; i++)
            {
                // Update message
                int currentMsg = i / msgInterval;
                if (currentMsg >= LoadingMessages.Length)
                    currentMsg = LoadingMessages.Length - 1;
                
                if (currentMsg != msgIndex)
                    msgIndex = currentMsg;
                
                // Draw progress bar
                int filled = (int)((i + 1) * 40.0 / totalSeconds);
                int empty = 40 - filled;
                string bar = new string('█', filled) + new string('░', empty);
                int percent = (int)((i + 1) * 100.0 / totalSeconds);
                
                // Move up to overwrite the progress lines
                if (i > 0)
                {
                    try { Console.SetCursorPosition(0, Console.CursorTop - 1); } catch { }
                }
                
                Console.WriteLine($"  [{bar}] {percent,3}%  {LoadingMessages[msgIndex],-40}");
                
                Thread.Sleep(1000);
            }
            
            Console.WriteLine();
            Console.WriteLine("  Loading complete.");
            Thread.Sleep(500);
        }
        
        public void DisplayHeader(string section)
        {
            DisplayHelper.ClearScreen();
            
            Console.WriteLine("============================================================");
            Console.WriteLine("                   HEAVENLY KEYS v3.0");
            Console.WriteLine("============================================================");
            Console.WriteLine($" COMPUTER : {SystemInfo.ComputerName}");
            Console.WriteLine($" OS       : {SystemInfo.OsVersion} {SystemInfo.Architecture}");
            Console.WriteLine($" ADMIN    : {(AdminHelper.IsAdministrator() ? "YES" : "NO")}");
            Console.WriteLine("============================================================");
            Console.WriteLine();
            Console.WriteLine($"                     {section}");
            Console.WriteLine();
            Console.WriteLine("============================================================");
        }
        
        public void ShowManager()
        {
            // No-op: ASCII art manager removed
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("  ASCII art features have been removed.");
            Console.WriteLine("============================================================");
            DisplayHelper.PauseReturnToMenu();
        }
        
        public void ReloadArtwork()
        {
            // No-op
        }
    }
}
