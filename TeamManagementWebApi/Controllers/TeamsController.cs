using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TeamManagementWebApi.Data;
using TeamManagementWebApi.Data.Entities;
using TeamManagementWebApi.Models;

namespace TeamManagementWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;


        public TeamsController(ITeamRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {

            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;

        }




        //get methods


        [HttpGet]
        public async Task<ActionResult<TeamModel[]>> Get(bool includePlayers = false)
        {
            try
            {
                var result = await _repository.GetAllTeamsAsync(includePlayers);
                return _mapper.Map<TeamModel[]>(result);
            }

            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }






        [HttpGet("{moniker}")]
            public async Task<ActionResult<TeamModel>> Get (string moniker)

        {
            try
            {
                var result = await _repository.GetTeamAsync(moniker);
                if (result == null) return NotFound();

                return _mapper.Map<TeamModel>(result);
            }

            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }









        [HttpGet("search")]
        public async Task<ActionResult<TeamModel[]>> SearchByDate (DateTime theDate, bool includePlayers =false)
        {
            try
            {
                var result = await _repository.GetAllTeamsByDateCreated(theDate, includePlayers);
                if (!result.Any()) return NotFound();
                return _mapper.Map<TeamModel[]>(result);
            }

            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }


        


        //post methods


   public async Task<ActionResult<TeamModel>> Post (TeamModel model)
        {
            try
            {
                var exisiting = await _repository.GetTeamAsync(model.Moniker);
                if (exisiting != null)
                {
                    return BadRequest("Moniker in use");
                }
                var location = _linkGenerator.GetPathByAction("Get", "Teams", new { moniker = model.Moniker });

                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could Not use the current Moniker");
                }

                //create  a new map
                var team = _mapper.Map<Team>(model);
                _repository.Add(team);

                if (await _repository.SaveChangesAsync())
                {
                    return Created($"/api/teams/{team.Moniker}",_mapper.Map<TeamModel>(team));
                }
            }

            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }






        //put method
        [HttpPut("{moniker}")]
        public async Task<ActionResult<TeamModel>> Put (string moniker, TeamModel model)
          {
            try
            {
                var oldTeam = await _repository.GetTeamAsync(moniker);
                if (oldTeam == null) return NotFound($"Could Not Found the moniker {moniker}");

                _mapper.Map(model, oldTeam);

                    if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<TeamModel>(oldTeam);
                }

            }

            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
    }
            return BadRequest();
}





        //delete method
        [HttpDelete("{moniker}")]
        public async Task<ActionResult> Delete(string moniker)
        {
            try
            {
                var oldTeam = await _repository.GetTeamAsync(moniker);
                if (oldTeam == null) return NotFound();

                _repository.Delete(oldTeam);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }

            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }

    }
}