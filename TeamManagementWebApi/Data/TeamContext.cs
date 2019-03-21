using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TeamManagementWebApi.Data.Entities;

namespace TeamManagementCamp.Data
{
    public class TeamContext : DbContext
    {
        private readonly IConfiguration _config;

        public TeamContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("TeamManagement"));
        }

        protected override void OnModelCreating(ModelBuilder bldr)
        {
            bldr.Entity<Team>()
              .HasData(new
              {
                  TeamId = 1,
                  Moniker = "Mil",
                  Name = "ACMilan",
                  DateTimeCreated = new DateTime(2018, 10, 18),
                  LocationId = 1,
                  TotalNumberOfPlayers = 25
              });

            bldr.Entity<Location>()
              .HasData(new
              {
                  LocationId = 1,
                  Stadium = "SanSiro",
                  Address = "123 Main Street",
                  CityName = "Milano",
                  PostalCode = "12345",
                  Country = "USA"
              });

            bldr.Entity<Player>()
              .HasData(new
              {
                  PlayerId = 1,
                  TeamId = 1,
                  PositionId = 1,
                  FirstName = "Alessio",
                  LastName = "Romagnoli",
                  Nationality = "Italian"
              },
              new
              {
                  PlayerId = 2,
                  TeamId = 1,
                  SpeakerId = 2,
                  FirstName = "Patrick",
                  LastName = "Cutrone",
                  Nationality = "Italian"
              });

            bldr.Entity<Position>()
              .HasData(new
              {
                  PositionId = 1,
                  PositionRole = "GoalKeeper",
                  NumberOfPlayers = 3,
                  CapacityIndex = true

              }, new
              {
                  PositionId = 2,
                  PositionRole = "Defender",
                  NumberOfPlayers = 7,
                  CapacityIndex = true
              });

        }

    }
}
