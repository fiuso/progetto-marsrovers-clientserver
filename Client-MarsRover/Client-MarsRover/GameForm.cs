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
using Client_MarsRover.Model;
using Client_MarsRover.SocketClient;

namespace Client_MarsRover
{
    public partial class GameForm : Form
    {
        private readonly GameController gameController;
        private readonly SocketRequestManager socketRequestManager;


        private Label[][] labels;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameController"></param>
        /// <param name="socketRequestManager"></param>
        public GameForm(GameController gameController, SocketRequestManager socketRequestManager)
        {

            InitializeComponent();

            this.gameController = gameController;
            this.socketRequestManager = socketRequestManager;

            this.flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight; //mette gli elementi in griglia e va  capo 

            if (this.gameController.GameMap != null)
                this.InitializeMap(this.gameController.GameMap.MapCenterdOnRover);

            var thread = new Thread(() => { // abbiamo il thread per modificare il game controller; (se c'� stata la modifca la aggiorno)
                while (this.socketRequestManager.Connection) {
                    if (gameController.ChangeWasCommited)
                    {
                        try
                        {
                            if (this.flowLayoutPanel1.Controls.Count == 0) //inizializzazione 
                                this.Invoke(new Action(() => {

                                    this.InitializeMap(gameController.GameMap.MapCenterdOnRover);
                                    this.UpdateGameStatus(gameController);

                                }));
                            else //aggiornamento del gioco 
                            {
                                this.Invoke(new Action(() => {

                                    this.UpdateMap(gameController.GameMap.MapCenterdOnRover);
                                    this.UpdateGameStatus(gameController);

                                }));

                            }
                            gameController.ReleaseChange();
                        }
                        catch (Exception e) { }
                    }
                    Thread.Sleep(100); // molto importante, da non togliere se non voglio bloccare il programma
                }
            });
            Thread connnectionThread = new Thread(() => {
                while(socketRequestManager.Connection) { }
                this.Invoke(() => this.Close());

             });

            connnectionThread.Start();
            thread.Start();

            this.FormClosing += (obj, e) => {           //quando questo evento viene lanciato 
                Environment.Exit(Environment.ExitCode);
            };

        }

        private void InitializeMap(CellElement[][] map)
        {
            if (map == null) return;
            labels = new Label[map.Length][];
            this.flowLayoutPanel1.Controls.Clear();
            for (int i = 0; i < map.Length; i++)
            {
                var labelsRow = new Label[map.Length];

                for (int j = 0; j < map.Length; j++)
                {

                    var label = new Label
                    {
                        BorderStyle = BorderStyle.FixedSingle,
                        Width = flowLayoutPanel1.Width / map.Length,
                        Height = flowLayoutPanel1.Width / map.Length,

                        Margin = new Padding(0, 0, 0, 0),
                    };

                    labelsRow[j] = label;
                    label.BackColor = map[i][j] switch
                    {
                        CellElement.Vuoto => Color.Gray,
                        CellElement.Giocatore => Color.Black,
                        CellElement.Ostacolo => Color.Aqua,
                        _ => Color.Red
                    };

                    flowLayoutPanel1.Controls.Add(label);
                }

                labels[i] = labelsRow;
            }
        }



        private void UpdateMap(CellElement[][] map)
        {
            if (map == null) return;
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map.Length; j++)
                {

                    labels[i][j].BackColor = map[i][j] switch
                    {
                        CellElement.Vuoto => Color.Gray,
                        CellElement.Giocatore => Color.Black,
                        CellElement.Ostacolo => Color.Aqua,
                        _ => Color.Red
                    };
                }
            }
        }


        private void UpdateGameStatus(GameController gameController)
        {

            switch (gameController.Life)
            {

                case 1:
                    pictureBox1.Visible = true;
                    pictureBox2.Visible = false;
                    pictureBox3.Visible = false;
                    break;
                case 2:
                    pictureBox1.Visible = true;
                    pictureBox2.Visible = true;
                    pictureBox3.Visible = false;
                    break;

                case 3:
                    pictureBox1.Visible = true;
                    pictureBox2.Visible = true;
                    pictureBox3.Visible = true;
                    break;

                default:
                    pictureBox1.Visible = false;
                    pictureBox2.Visible = false;
                    pictureBox3.Visible = false;
                    break;
            }

            if (gameController.GameOver)
            {
                var dialog = new ShowDialog(socketRequestManager);
                dialog.ShowDialog();
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            socketRequestManager.EnqueueRequest(RequestType.MOVE, Direction.UP);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            socketRequestManager.EnqueueRequest(RequestType.MOVE, Direction.DOWN);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            socketRequestManager.EnqueueRequest(RequestType.MOVE, Direction.RIGHT);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            socketRequestManager.EnqueueRequest(RequestType.MOVE, Direction.LEFT);

        }
    }
}
