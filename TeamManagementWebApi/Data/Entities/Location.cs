using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamManagementWebApi.Data.Entities
{
    public class Location
    {
        public int LocationId { get; set; }
        public string StadiumName { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
