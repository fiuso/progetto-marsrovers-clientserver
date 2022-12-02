using Server_MarsRover.Controller;
using Server_MarsRover.SocketServer;

namespace Server_MarsRover.Model
{

    public class Map {
        private class Position {
            public int X { get; set; }
            public int Y { get; set; }
        }

        public int RoverX { get => this.roverPosition.X; }
        public int RoverY { get => this.roverPosition.Y; }
        
        public CellElement[][] MapCenterdOnRover { get => this.ExtractNxNMap(15); }

        private CellElement[][] ExtractNxNMap(int n){                      //non ho tutta la mappa ma prendo solo parte della mappa 
         
            var returnArray = new CellElement[n][];
            for (var i = RoverX - n / 2; i <= RoverX + n / 2; i++) { //queste sono le colonne (tutti i valori)

                var currentArray = new CellElement[n];
                for (var j = RoverY - n / 2; j <= RoverY + n / 2; j++) { //queste sono di righe(tutti i valori)
                    currentArray[j + n / 2 - RoverY] = array[i][j];
                }

                returnArray[i + n / 2 - RoverX] = currentArray;
            }

            return returnArray;
        }

        private Position roverPosition;
        CellElement[][] array; // sappiamo essere quadrata
        
        public Map(CellElement[][] array) {                                     //creazione di una mappa e creazione posizione giocatore  (mappa generic) 
            
            this.array = array;

            Random rand = new Random();
            int x, y;
            
            this.roverPosition = new Position{ X = 16, Y = 16 };
            this.array[roverPosition.X][roverPosition.Y] = CellElement.Giocatore;
            // Bordo fatto creazione di ostacoli

            for(var i = 0; i < 15 / 2; i++) {
                for(var j = 0; j < this.array.Length; j++) {
                    this.array[i][j] = CellElement.Ostacolo;
                    this.array[j][i] = CellElement.Ostacolo;
                }
            }

            for(var i = this.array.Length - 15 / 2; i < this.array.Length; i++) {
                for (var j = 0; j < this.array.Length; j++) {
                    this.array[i][j] = CellElement.Ostacolo;
                    this.array[j][i] = CellElement.Ostacolo;
                }
            }
        }


        public CellElement[][] GetArray() {
            return array;
        }

 

        //movimenti robottino + controlli 
        //aiutato sui calcoli da applicare per lo spostamento
        public bool MuoviRobot(Direction direzione) {
            switch(direzione) {
                case Direction.UP:
                    // - 1 x
                    var newXPosition = roverPosition.X - 1;
                    if (newXPosition < 0) return false;
                    // Controlla se c'è un ostacolo nella posizione in cui vogliamo andare lo blocchiamo e prende danno.
                    if (array[newXPosition][RoverY] == CellElement.Ostacolo) { return false; }

                    // Opzionale visto che non abbiamo molteplici rover effetivamente quello che muoviamo può anche non occupare una cella veramente.
                    this.array[newXPosition][this.RoverY] = CellElement.Giocatore;
                    this.array[RoverX][RoverY] = 0;
                    
                    roverPosition.X = newXPosition;
                    return true;

                case Direction.DOWN: 
                    // + 1 x
                    var newXPosizione = roverPosition.X + 1;
                    // Controlla se c'è un ostacolo nella posizione in cui vogliamo andare * lo blocchiamo e (prende danno).
                    if (newXPosizione > array[0].Length) return false;    //controllo effettivo se <0 

                    if(array[newXPosizione][RoverY]== CellElement.Ostacolo) { return false; }

                    this.array[newXPosizione][this.RoverY] = CellElement.Giocatore;
                    this.array[RoverX][RoverY] = CellElement.Vuoto;

                    roverPosition.X = newXPosizione;
                    return true;

                case Direction.RIGHT: 
                    // y + 1
                    var newYPosition = roverPosition.Y + 1;
                    
                    if(newYPosition > array[0].Length) return false;
                    if(array[RoverX][newYPosition]== CellElement.Ostacolo) { return false; }

                    this.array[this.RoverX][newYPosition] = CellElement.Giocatore;
                    this.array[RoverX][RoverY] = CellElement.Vuoto;

                    roverPosition.Y = newYPosition;
                    return true;
                case Direction.LEFT:
                    // y - 1
                    var newYposizione = roverPosition.Y - 1;

                    if(newYposizione < 0) return false;
                    if(array[RoverX][newYposizione]== CellElement.Ostacolo) { return false; }

                    this.array[this.RoverX][newYposizione] = CellElement.Giocatore;
                    this.array[RoverX][RoverY] = CellElement.Vuoto;

                    roverPosition.Y = newYposizione;
                    return true;
         
                default: break;
            }

            return false;
        }
    }
}
