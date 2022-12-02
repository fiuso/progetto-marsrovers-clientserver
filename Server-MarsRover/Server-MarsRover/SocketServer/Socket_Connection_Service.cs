using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Server_MarsRover.Controller;
using Server_MarsRover.Log;

namespace Server_MarsRover.SocketServer
{
    internal class Socket_Connection_Service
    {

        private readonly Socket socket;
        private readonly GameController controller;

        private readonly string _eom;

        private readonly SocketLog socketLog;

        public Socket_Connection_Service(Socket socket, GameController gameController, SocketLog socketLog, string eom = "<|EOM|>") {

            this.socket = socket;
            this.controller = gameController;

            this._eom = eom;
            this.socketLog = socketLog;

        }


        public async void DataReceiveHandler()
        {
            while (true) {
                try
                {

                    var request = await ExecuteRequest(socket);

                    if (request != null) {

                        switch (request.Request.RequestType)
                        {
                            case RequestType.MOVE:
                                this.controller.MuoviRobot(request.Request.Direction); break;
                            case RequestType.CONNECT:
                                this.controller.InitializeGame();
                                break;
                            case RequestType.DISCONNECT: break;
                        }
                        await this.SendResponse(request);
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }

            }
        }

        private async Task<SocketRequest> ExecuteRequest(Socket socket) {

            var buffer = new byte[1_024];

            var received = await socket.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);
            if (response.IndexOf(_eom) > -1) {
                var stringSplitElements = new string[] { this._eom };

                var request = JsonSerializer.Deserialize<SocketRequest>(response.Split(stringSplitElements, StringSplitOptions.None)[0]);
                this.socketLog.AddEntry(request.Request.ToString(), $"Client: {socket.RemoteEndPoint.ToString()}", request.Request.RequestType);
                return request;
            }

            return null;
        }

        private async Task<object> SendResponse(SocketRequest request) {
            request.Status = new Status { GameController = this.controller };
            var outputString = JsonSerializer.Serialize(request) + this._eom;

            var echoBytes = Encoding.UTF8.GetBytes(outputString);
            this.socketLog.AddEntry(outputString, $"Server: {socket.LocalEndPoint.ToString()}", 
                request.Request.RequestType == RequestType.CONNECT ? RequestType.DATA : RequestType.MOVE);

            await socket.SendAsync(echoBytes, 0);
            return null;
        }
    }
}
