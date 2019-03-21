using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamManagementWebApi.Models
{
    public class TeamModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string Moniker { get; set; }
        
        public DateTime DateTimeCreated { get; set; }
        [Range(20,30)]
        public int TotalNumberOfPlayers { get; set; }


        public string Stadium { get; set; }
        public string LocationStadium { get; set; }
        public string LocationAddress { get; set; }
        public string LocationCityName { get; set; }
        public string LocationPostalCode { get; set; }
        public string LocationCountry { get; set; }

        public ICollection<PlayerModel> Players { get; set; }
    }
}
