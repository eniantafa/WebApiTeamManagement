using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamManagementWebApi.Data.Entities;

namespace TeamManagementWebApi.Data
{
    public interface ITeamRepository
    {
        // General 
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // Teams
        Task<Team[]> GetAllTeamsAsync(bool includePlayers = false);
        Task<Team> GetTeamAsync(string moniker, bool includePlayers = false);
        Task<Team[]> GetAllTeamsByDateCreated(DateTime dateTime, bool includePlayers = false);

        // Players
        Task<Player> GetPlayerByMonikerAsync(string moniker, int playerId, bool includePositions = false);
        Task<Player[]> GetPlayersByMonikerAsync(string moniker, bool includePositions = false);

        // Positions
        Task<Position[]> GetPositionsByMonikerAsync(string moniker);
        Task<Position> GetPositionAsync(int positionId);
        Task<Position[]> GetAllPositionsAsync();

    }
}