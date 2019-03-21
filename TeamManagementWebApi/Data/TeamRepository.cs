using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamManagementCamp.Data;
using TeamManagementWebApi.Data;
using TeamManagementWebApi.Data.Entities;

namespace TeamManagementWebApi.Data
{
    public class TeamRepository : ITeamRepository
    {
        private readonly TeamContext _context;
        private readonly ILogger<TeamRepository> _logger;

        public TeamRepository(TeamContext context, ILogger<TeamRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Adding an object of type {entity.GetType()} to the context.");
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _logger.LogInformation($"Removing an object of type {entity.GetType()} to the context.");
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempitng to save the changes in the context");

            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Team[]> GetAllTeamsByDateCreated(DateTime dateTime, bool includePlayers = false)
        {
            _logger.LogInformation($"Getting all Teams");

            IQueryable<Team> query = _context.Teams
                .Include(c => c.Location);

            if (includePlayers)
            {
                query = query
                  .Include(c => c.Players)
                  .ThenInclude(t => t.Position);
            }

            // Order It
            query = query.OrderByDescending(c => c.DateTimeCreated)
              .Where(c => c.DateTimeCreated.Date == dateTime.Date);

            return await query.ToArrayAsync();
        }

        public async Task<Team[]> GetAllTeamsAsync(bool includePlayers = false)
        {
            _logger.LogInformation($"Getting all Teams");

            IQueryable<Team> query = _context.Teams
                .Include(c => c.Location);

            if (includePlayers)
            {
                query = query
                  .Include(c => c.Players)
                  .ThenInclude(t => t.Position);
            }

            // Order It
            query = query.OrderByDescending(c => c.DateTimeCreated);

            return await query.ToArrayAsync();
        }

        public async Task<Team> GetTeamAsync(string moniker, bool includePlayers = false)
        {
            _logger.LogInformation($"Getting a Team for {moniker}");

            IQueryable<Team> query = _context.Teams
                .Include(c => c.Location);

            if (includePlayers)
            {
                query = query.Include(c => c.Players)
                  .ThenInclude(t => t.Position);
            }

            // Query It
            query = query.Where(c => c.Moniker == moniker);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Player[]> GetPlayersByMonikerAsync(string moniker, bool includePlayers = false)
        {
            _logger.LogInformation($"Getting all Players for a Team");

            IQueryable<Player> query = _context.Players;

            if (includePlayers)
            {
                query = query
                  .Include(t => t.Position);
            }

            // Add Query
            query = query
              .Where(t => t.Team.Moniker == moniker)
              .OrderByDescending(t => t.LastName);

            return await query.ToArrayAsync();
        }

        public async Task<Player> GetPlayerByMonikerAsync(string moniker, int playerId, bool includePositions = false)
        {
            _logger.LogInformation($"Getting all Players for a Team");

            IQueryable<Player> query = _context.Players;

            if (includePositions)
            {
                query = query
                  .Include(t => t.Position);
            }

            // Add Query
            query = query
              .Where(t => t.PlayerId == playerId && t.Team.Moniker == moniker);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Position[]> GetPositionsByMonikerAsync(string moniker)
        {
            _logger.LogInformation($"Getting all Speakers for a Camp");

            IQueryable<Position> query = _context.Players
              .Where(t => t.Team.Moniker == moniker)
              .Select(t => t.Position)
              .Where(s => s != null)
              .OrderBy(s => s.PositionRole)
              .Distinct();

            return await query.ToArrayAsync();
        }

        public async Task<Position[]> GetAllPositionsAsync()
        {
            _logger.LogInformation($"Getting Position");

            var query = _context.Positions
              .OrderBy(t => t.PositionRole);

            return await query.ToArrayAsync();
        }


        public async Task<Position> GetPositionAsync(int positionId)
        {
            _logger.LogInformation($"Getting Position");

            var query = _context.Positions
              .Where(t => t.PositionId == positionId);

            return await query.FirstOrDefaultAsync();
        }

    }
}
