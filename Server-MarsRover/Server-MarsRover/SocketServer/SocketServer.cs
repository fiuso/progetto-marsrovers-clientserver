using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server_MarsRover.Controller;
using Server_MarsRover.Log;

/// <summary>
///     decidere come funzioa la comunicazione:
///     
///     keyword : game, move, connect : {}, close : {}
/// 
///     Da Server a Client:
///         + Mandiamo Game Status con vita del rover e magari qualos'altro (probabilmente la mappa sta qui d        
///         + Mandiamo la mappa 8x8 centrata sul robottino + vita rimasta del robot.
///         -> Il Client potrebbe volendo rispondere (ACK) opzionale
///         + Resettare la partita -> Manda nuova mappa e tutto
///         -> Messaggio di GameOver
/// 
///     Da Client a Server
///         + Comando di movimento ->  trovare una "Codifica"
///             "game": {
///                 "move" : {
///                     "X" : 1, "Y": 0
///                 }
///             }    
///             
///             {
///                 "move" : 1, 2, 3, 4 (con Enum tra client e server) -> Per essere sicuri che non ci siano ambiguità quando il client
///                 si connettee gli mandiamo la enum del tipo  
///                 {
///                     Destra :1,
///                     Sinistra:2,
///                     Su: 0,
///                     Giu: 1
///                 }
///             }
///             
///         + Richiesta di connessione (base)
///         + Richiesta di stop del giocatore (disconnessione) (base)
///         + Richiesta di "Continua a giocare" in caso di game over -> La partita si resetta solo dopo questa risposta.
/// 
/// </summary>

namespace Server_MarsRover.SocketServer
{
    internal class SocketServer {


        private Socket socket;
        private readonly GameController controller;
        private readonly SocketLog socketLog;
        public SocketServer(Socket socket) { 
            this.socket = socket;
        }

        public SocketServer(int portNumber, string ipAddress, GameController gameController, SocketLog socketLog) {

            System.Net.IPAddress address = System.Net.IPAddress.Parse(ipAddress);
            System.Net.IPEndPoint localEndPoint = new System.Net.IPEndPoint(address, portNumber);
            this.socketLog = socketLog;

            this.socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.socket.Bind(localEndPoint);


            controller = gameController;
        }


        public async void InitializeServer() {
           
            try {

                this.socket.Listen(1);          // server è in ascolto di un client 
               
                while(true) {

                    var currentConnection = await this.socket.AcceptAsync();

                    Socket_Connection_Service clientThread = new Socket_Connection_Service(currentConnection, controller, this.socketLog);
                    Thread thread = new Thread(new ThreadStart(clientThread.DataReceiveHandler));       //faccio partire un thread all'interno dopo la connessipme
                    thread.Start();
                }

            } catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }



    }
}
