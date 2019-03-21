using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamManagementWebApi.Data.Entities
{
    public class Team
    {

        public int TeamId { get; set; }
        public string Moniker { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public int TotalNumberOfPlayers { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}
