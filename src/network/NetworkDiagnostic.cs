using System;
using System.Net;
using System.Net.NetworkInformation;
using USBUnlocker.Utilities;

namespace USBUnlocker.Network
{
    public class NetworkDiagnostic
    {
        public void Run()
        {
            DisplayHelper.ClearScreen();
            Console.WriteLine("============================================================");
            Console.WriteLine("                     NETWORK DIAGNOSTIC");
            Console.WriteLine("============================================================\n");
            
            // Check adapters
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("NETWORK ADAPTERS");
            Console.WriteLine("------------------------------------------------------------");
            
            try
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface iface in interfaces)
                {
                    if (iface.OperationalStatus == OperationalStatus.Up)
                    {
                        Console.WriteLine($"  {iface.Name}");
                        Console.WriteLine($"  Status:  {iface.OperationalStatus}");
                        Console.WriteLine($"  Type:    {iface.NetworkInterfaceType}");
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error: " + ex.Message + "\n");
            }
            
            // Check gateway
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("GATEWAY");
            Console.WriteLine("------------------------------------------------------------");
            
            try
            {
                IPInterfaceProperties props = NetworkInterface.GetAllNetworkInterfaces()[0].GetIPProperties();
                foreach (var gateway in props.GatewayAddresses)
                {
                    Console.WriteLine($"  {gateway.Address}");
                }
            }
            catch
            {
                Console.WriteLine("  No gateway detected");
            }
            Console.WriteLine();
            
            // Check DNS
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("DNS");
            Console.WriteLine("------------------------------------------------------------");
            
            try
            {
                IPHostEntry host = Dns.GetHostEntry("www.google.com");
                Console.WriteLine($"  DNS Resolution: OK ({host.AddressList[0]})");
            }
            catch
            {
                Console.WriteLine("  DNS Resolution: FAILED");
            }
            Console.WriteLine();
            
            // Ping test
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("CONNECTIVITY");
            Console.WriteLine("------------------------------------------------------------");
            
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send("8.8.8.8", 3000);
                    if (reply.Status == IPStatus.Success)
                        Console.WriteLine("  Internet:     OK");
                    else
                        Console.WriteLine("  Internet:     FAILED");
                }
            }
            catch
            {
                Console.WriteLine("  Internet:     UNAVAILABLE");
            }
            
            Console.WriteLine("\n============================================================");
        }
    }
}
