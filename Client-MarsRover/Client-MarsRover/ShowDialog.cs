using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client_MarsRover.SocketClient;

namespace Client_MarsRover
{
    public partial class ShowDialog : Form
    {
        private readonly SocketRequestManager requestManager;
        public ShowDialog(SocketRequestManager socketRequestManager) {
            InitializeComponent();
            this.requestManager = socketRequestManager;
            label1.Text = " Game Over";
        }

        private void button2_Click(object sender, EventArgs e) {
            this.requestManager.EnqueueRequest(RequestType.CONNECT, Direction.LEFT);
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e) {
            Environment.Exit(1);
        }

        private void label1_Click(object sender, EventArgs e){
        }

        private void ShowDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
