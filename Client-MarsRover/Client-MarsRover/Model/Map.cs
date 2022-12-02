using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_MarsRover.Model {
    public class Map {
        private class Position {
            public int X { get; set; }
            public int Y { get; set; }
        }

        public int RoverX { get => this.roverPosition.X; set => this.roverPosition.X = value; }
        public int RoverY { get => this.roverPosition.Y; set => this.roverPosition.Y = value; }
        private Position roverPosition = new Position();
        public CellElement [][] MapCenterdOnRover { get; set; }  
    }
}
