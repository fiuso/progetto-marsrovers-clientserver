using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server_MarsRover.Controller;

namespace Server_MarsRover.SocketServer {
    internal class SocketRequest {                  //le richieste che possono arrivare possono essere di 2 tipi. il movimento, connessione e disconnessione e la tipologia di direzione in base al MOVE
        public Request Request { get; set; }
        public Status Status { get; set; }
    }


    public enum RequestType {
        MOVE, CONNECT, DISCONNECT, DATA
    }

    public enum Direction {
        LEFT, RIGHT, UP, DOWN
    }

    internal class Request {
        public RequestType RequestType { get; set; }
        public Direction Direction { get; set; }
    }

    internal class Status {
        public GameController GameController { get; set; }
    }
}
