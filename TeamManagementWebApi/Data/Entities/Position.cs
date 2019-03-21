using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamManagementWebApi.Data.Entities
{
    public class Position
    {

        public int PositionId { get; set; }
        public string PositionRole { get; set; }
        public int NumberOfPlayers { get; set; }
        public bool CapacityIndex { get; set; }
    }
}
