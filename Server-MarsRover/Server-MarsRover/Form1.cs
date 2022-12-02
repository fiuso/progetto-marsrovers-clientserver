using Server_MarsRover.Controller;
using Server_MarsRover.Log;

namespace Server_MarsRover
{
    public partial class Form1 : Form
    {

        public Form1(int port, string address, GameController gameController, SocketLog socketLog) {

            InitializeComponent();
            this.label3.Text = $"{address}:{port}";

            this.button1.Click += (o, arg) => {
                var socketServer = new SocketServer.SocketServer(port, address, gameController, socketLog);
                Thread thread = new Thread(new ThreadStart(socketServer.InitializeServer));
                thread.Start();
                button1.Enabled = false;
                
                socketLog.Clear();
            };


            var logThread = new Thread(() => {
                var currentlyAddedCommands = 0;
                while (true)
                {

                    if (currentlyAddedCommands < socketLog.CommandsLength)
                    {
                        for (var i = currentlyAddedCommands; i < socketLog.CommandsLength; i++)
                        {
                            var currentElement = socketLog.Commands.ToList()[i];
                            try {
                                this.Invoke(() => this.listBox1.Items.Add($"{currentElement.Sender}>> REQUEST: {currentElement.Request} - {currentElement.Text}"));
                            } catch{}
                        }
                        currentlyAddedCommands = socketLog.CommandsLength;

                    }
                }
            });

            logThread.Start();
            this.FormClosing += (obj, ar) => {
                try
                {
                    logThread.Interrupt();
                    Environment.Exit(Environment.ExitCode);
                }
                catch { }
            };

        }
    }
}