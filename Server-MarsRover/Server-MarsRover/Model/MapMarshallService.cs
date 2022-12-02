using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Server_MarsRover.Model
{
    internal class MapMarshallService {
        public Map Unmarshall(string encodedMap) {
            return new Map(null);
        }

        // max 8 x 8
        public string Marshall(Map map) {   //sto convertendo i dati in Json
            return JsonSerializer.Serialize(map);
        }
    }
}
