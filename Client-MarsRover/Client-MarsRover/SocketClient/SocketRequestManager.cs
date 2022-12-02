using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Client_MarsRover.Controller;

namespace Client_MarsRover.SocketClient {
    public class SocketRequestManager {

        private GameController gameController;
        private bool _pendingRequest = false;
        private Queue<Request> _requests = new Queue<Request>();  //coda di richieste
        public bool Connection { get; set; } = true;
        public SocketRequestManager(GameController gameController) {
            this.gameController = gameController;
        }
        public void EnqueueRequest(RequestType requestType, Direction direction) {
            
            var request = new Request {
                Direction = direction,
                RequestType = requestType
            };

            this._requests.Enqueue(request); // aggiunge l'elemento alla fine della queue 
        }


        public async Task<bool> SendQueuedRequest(Socket socket) {

            if (_requests.Count == 0) return true;   //controllo di richieste va a ritornare niente se ci sono zero richieste se no conta le richieste
            try {
                var result = await this.ExecuteRequest(socket); //risultato della richiesta che ci da anche la mappa
                if (result == null) return false;

                this.gameController.Life = result.Status.GameController.Life;
                this.gameController.GameMap = result.Status.GameController.GameMap;
                this.gameController.GameOver = result.Status.GameController.GameOver;
                return true;

            } catch (Exception ex) {
                this.Connection = false;
                return false;
            }
         }

        /// <summary>
        ///     Come regola lavora sul primo
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private async Task<SocketRequest> ExecuteRequest(Socket socket) {

            var buffer = new byte[2_048];
            if (!_pendingRequest) //controlla se c'è gia una richiesta e se c'è già la va a saltare
            {
                // teoricamnete siamo fuori dal nostro standard interno
                var request = _requests.Peek();

                var socketRequest = new SocketRequest { Request = _requests.Peek() };
                var sendString = JsonSerializer.Serialize(socketRequest) + "<|EOM|>"; //converte i dati in json + EOM

                _ = await socket.SendAsync(Encoding.UTF8.GetBytes(sendString), SocketFlags.None); // mandata la stringa in byte 
                Console.WriteLine($"Socket client sent message: \"{sendString}\"");
                _pendingRequest = true;
            }

            var received = await socket.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);

            if (response.Contains("<|EOM|>")) //  controllare che c'è il terninatore 
            {

                var stringSplitElements = new string[] { "<|EOM|>" };
                var jsonText = response.Split(stringSplitElements, StringSplitOptions.None)[0]; 

                var request = JsonSerializer.Deserialize<SocketRequest>(jsonText);

                // Json scomposizione e roba dei dati. L'azione della chiamata è qui in realta.

                Console.WriteLine($"Socket client received acknowledgment: \"{jsonText}\"");
                _requests.Dequeue(); // metto la richiesta in fondo alla coda di queue 
                _pendingRequest = false;

                return request;
            }

            return null;
        }

    }
}
