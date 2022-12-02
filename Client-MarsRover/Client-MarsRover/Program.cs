using System.Text.Json;
using Client_MarsRover.Controller;
using Client_MarsRover.Model;
using Client_MarsRover.SocketClient;

namespace Client_MarsRover
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            System.Diagnostics.Debug.WriteLine("PROVA");

            var gameController = new GameController {
                GameMap = new Map { }
            };

            var requestManager = new SocketRequestManager(gameController);
            requestManager.EnqueueRequest(RequestType.CONNECT, Direction.LEFT);

            Application.Run(new SocketConnect(gameController, requestManager));
        }
    }
}