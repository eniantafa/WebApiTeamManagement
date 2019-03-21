using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamManagementWebApi.Models
{
    public class PlayerModel
    {
       [Required]
       [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100,MinimumLength =1)]
        public string Nationality { get; set; }
        public PositionModel Position { get; set; }

    }
}
