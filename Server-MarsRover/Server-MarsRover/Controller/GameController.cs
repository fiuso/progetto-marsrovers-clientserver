using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server_MarsRover.Model;
using Server_MarsRover.SocketServer;

namespace Server_MarsRover.Controller {
    public class GameController {

        private const int MapSize = 32;                             // grandezza della mappa 
        private const int MapObstacles = MapSize * MapSize / 3 + 1; //decisione senza motivo di aggiungere 1 

        private const int MaxLife = 3;
        public int Life { get; private set; } = MaxLife;
        public Map? GameMap { get; private set; } 
        public bool GameOver { get => Life <= 0;  }


        public void InitializeGame(){         // inizializzazione del gioc

            CellElement[][] grid = new CellElement[MapSize][]; // posizione con una matrice (griglia) array di 32 * y  (contenitore vuoto di righe)
            for(int i =0; i < MapSize; i++) { grid[i] = new CellElement[MapSize]; } //32*32  (aggiungiamo tutte le righe: defolt)

            for (int i = 0; i < MapObstacles; i++){

                Random rand = new Random();         //randomico  
                int x, y;

                do{
                
                    x = rand.Next(MapSize);
                    y = rand.Next(MapSize);
                
                } while (grid[x][y] == CellElement.Ostacolo);           //continuo finchè la posizione non è libera 
                grid[x][y] = CellElement.Ostacolo; //osrtacolo messo in  griglia
            }

            this.GameMap = new Map(grid);
            this.Life = MaxLife;
        }
        public GameController() {
            this.InitializeGame();
        }

        // eccezione in caso di fallimento
        public void MuoviRobot(Direction direzione) {

            if (GameMap is null) throw new Exception("Map not initialized");
            var moved = GameMap.MuoviRobot(direzione);
            if (!moved) Life--;

        }

    }

    public enum Direzione {
        Avanti, Indietro, Destra, Sinistra
    }
}
