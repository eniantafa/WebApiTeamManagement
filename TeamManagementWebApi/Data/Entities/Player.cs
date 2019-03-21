using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamManagementWebApi.Data.Entities
{
    public class Player
    {
        public int PlayerId { get; set; }
        public Team Team { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public Position Position { get; set; }
       // public Location Location { get; set; }
        
    }
}
