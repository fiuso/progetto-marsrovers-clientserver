using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Server_MarsRover.Controller;

namespace Server_MarsRover
{
    internal static class Program
    {

        public static string GetLocalIPAddress()        //funzione già esistente presa da internet
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
           
            GameController game = new GameController();
            game.InitializeGame();
            
            var localAddress = GetLocalIPAddress();
            var localPort = 900;
        

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(localPort, localAddress, game, new Log.SocketLog()));
        }
    }
}