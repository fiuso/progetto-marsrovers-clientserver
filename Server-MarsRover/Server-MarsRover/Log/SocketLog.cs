using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server_MarsRover.SocketServer;

namespace Server_MarsRover.Log
{

    public record LogEntry(string Text, string Sender, RequestType Request);

    public class SocketLog {

        private List<LogEntry> _commands = new List<LogEntry>();
        public int CommandsLength { get => _commands.Count; }

        public void AddEntry(string Text, string Sender, RequestType Request) {
            this._commands.Add(new LogEntry(Text, Sender, Request));
        }

        public IReadOnlyCollection<LogEntry> Commands { get => _commands; } 
        public void Clear() { _commands.Clear(); }

    }
}
