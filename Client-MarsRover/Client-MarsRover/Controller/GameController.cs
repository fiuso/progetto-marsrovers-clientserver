using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client_MarsRover.Model;

namespace Client_MarsRover.Controller {
    public class GameController {


        private object key = new object();
        public bool ChangeWasCommited { get; private set; }
        private int _life;
        public int Life {
            get { return _life; }
            set
            {
                _life = value;
                ChangeWasCommited = true;
            }
        }

        private Map _gameMap;
        public Map GameMap { 
            get { return _gameMap; }
            set {
                _gameMap = value;
                ChangeWasCommited = true;
            }
        }

        private bool _gameOver;
        public bool GameOver {
            get { return _gameOver; }
            set {
                _gameOver = value;
                ChangeWasCommited = true;
            }
        }

        public void ReleaseChange() {
            lock(key) { //si spera ci sia solo un thread  (window thread) 
                this.ChangeWasCommited = false;
            }
        }
    }
}
