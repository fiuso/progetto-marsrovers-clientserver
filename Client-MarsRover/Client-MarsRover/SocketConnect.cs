using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client_MarsRover.Controller;
using Client_MarsRover.SocketClient;

namespace Client_MarsRover
{
    public partial class SocketConnect : Form
    {
        private readonly GameController gameController;
        private readonly SocketRequestManager socketRequestManager;
        
        public SocketConnect(GameController gameController, SocketRequestManager requestManager) {
            InitializeComponent();
            this.socketRequestManager = requestManager;
            this.gameController = gameController;

        }
        private SocketClient.SocketClient socketClient;
        private void button1_Click(object sender, EventArgs e) {

            try
            {
                //aiutato da un esterno 
                var timeout = false;
                this.button1.Enabled = false;
                var task = Task.Run(() => {
                    Thread.Sleep(2500);
                    this.Invoke(() => this.button1.Enabled = true);
                    timeout = true;
                });

                this.socketClient = new SocketClient.SocketClient(int.Parse(this.textBox2.Text), this.textBox1.Text, this.socketRequestManager);

                Thread comunicationThread = new(new ThreadStart(socketClient.MakeConnection));
                comunicationThread.Start();

                Thread openNewFormThread = new(() =>
                {
                    var isConnected = false;

                    while (!isConnected)
                    {
                        if (timeout) return;
                        if (socketClient.isConnected) {
                            GameForm gameForm = new GameForm(gameController, socketRequestManager);
                            gameForm.ShowDialog();
                            isConnected = true;
                        }
                    }

                });

                openNewFormThread.Start();
            } catch { }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SocketConnect_Load(object sender, EventArgs e)
        {

        }
    }
}
