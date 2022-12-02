using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client_MarsRover.SocketClient
{
    internal class SocketClient {

        private Socket socket;
        private System.Net.IPEndPoint _endPoint;

        private SocketRequestManager requestManager;
        public bool isConnected = false;
        public SocketClient(int portNumber, string ipAddress, SocketRequestManager socketRequestManager){

            System.Net.IPAddress address = System.Net.IPAddress.Parse(ipAddress);
            this._endPoint  = new System.Net.IPEndPoint(address, portNumber);

            this.socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.requestManager = socketRequestManager;
        }

       
        public async void MakeConnection() {
            try {

                var cts = new CancellationTokenSource();
                cts.CancelAfter(2000);

                await this.socket.ConnectAsync(this._endPoint, cts.Token);

                this.requestManager.Connection = true;
                this.isConnected = true;


                while (this.isConnected) {
                    try  {
                        if (this.requestManager != null) {
                            var result = await this.requestManager.SendQueuedRequest(this.socket);
                            this.requestManager.Connection = result;
                        }
                        Thread.Sleep(100);

                    } catch (Exception exception) {
                        Console.WriteLine(exception.ToString());
                        this.isConnected = false;

                    }
                }
                
            } catch (Exception exception) {
                Console.WriteLine(exception.ToString());
                this.isConnected = false;
                this.requestManager.Connection = false;
            }
        }

    }
}
