using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Client_MarsRover.Controller;

namespace Client_MarsRover.SocketClient {
    /// <summary>
    /// 
    /// </summary>
    internal class SocketRequest {
        // JSON Properties
        public Request Request { get; set; }
        public Status Status { get; set; }
    }


    public enum RequestType { 
        MOVE, CONNECT, DISCONNECT
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
