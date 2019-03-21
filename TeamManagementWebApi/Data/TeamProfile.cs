using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamManagementWebApi.Data.Entities;
using TeamManagementWebApi.Models;

namespace TeamManagementWebApi.Data
{
    public class TeamProfile: Profile
    {
        public TeamProfile()
        {
            this.CreateMap<Team, TeamModel>()
                .ForMember(c => c.Stadium, o => o.MapFrom(m => m.Location.StadiumName))
                .ReverseMap();

            this.CreateMap<Player, PlayerModel>()
                .ReverseMap()
                .ForMember(t => t.Team, opt => opt.Ignore())
                .ForMember(t => t.Position, opt => opt.Ignore());

            this.CreateMap<Position, PositionModel>()
                .ReverseMap();
                
        }
    }
}
