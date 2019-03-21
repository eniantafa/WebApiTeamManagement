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
    public class PlayersController : ControllerBase
    {
        private readonly ITeamRepository repository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;



        public PlayersController(ITeamRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
        }



        //get methods
        public async Task<ActionResult<PlayerModel[]>> Get(string moniker)
        {
            try
            {
                var players = await repository.GetPlayersByMonikerAsync(moniker);
                if (players == null) return NotFound();

                return mapper.Map<PlayerModel[]>(players);
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to connect to the database");
            }
        }





        [HttpGet("{id:int}")]
            public async Task<ActionResult<PlayerModel>> Get(string moniker, int id)
        {
            try
            {
                var team = await repository.GetTeamAsync(moniker);
                if (team == null) return NotFound("Team doesn't found");

                var player = await repository.GetPlayerByMonikerAsync(moniker, id);

                if (player == null) return NotFound("Player not found");

                return mapper.Map<PlayerModel>(player);
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to connect to the database");
            }
        }




        //post method

        public async Task<ActionResult<PlayerModel>> Post(string moniker, PlayerModel model)
        {
            try
            {
 

                var team = await repository.GetTeamAsync(moniker);
                if (team == null) return NotFound("Team doesn't found");

                var player = mapper.Map<Player>(model);

                player.Team = team;
                if (model.Position == null) return BadRequest("Team doesn't exist");
                var position = await repository.GetPositionAsync(model.Position.PositionId);
                if (position == null) return BadRequest("Position could not be found");
                player.Position = position;

                repository.Add(player);


                if (await repository.SaveChangesAsync())
                {
                    var url = linkGenerator.GetPathByAction(HttpContext, "Get", values: new
                    {
                        moniker,
                        id = player.PlayerId
                    });
                    return Created(url, mapper.Map<PlayerModel>(player));
                }
                else
                {
                    return BadRequest("Failed to save new talk");
                }
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to connect to the database");
            }
        }





        //put


        [HttpPut("{id:int}")]
        public async Task<ActionResult<PlayerModel>> Put(string moniker, int id, PlayerModel model)
        {
            try
            {
                var player = await repository.GetPlayerByMonikerAsync(moniker, id);
                if (player == null) return NotFound("Couldn't find the player");

                if (model.Position != null)
                {
                    var position = await repository.GetPositionAsync(model.Position.PositionId);
                    player.Position = position;
                }

                mapper.Map(model, player);

                if (await repository.SaveChangesAsync())
                {
                    return mapper.Map<PlayerModel>(player);
                }
                return BadRequest();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to connect to database");
            }
        }







        //delete

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(string moniker, int id)
        {

            try
            {
                var player = await repository.GetPlayerByMonikerAsync(moniker, id);
                if (player == null) return NotFound("Failed to find the player to delete");

                repository.Delete(player);

                if (await repository.SaveChangesAsync())
                {

                    return Ok();
                }

                return BadRequest("Failed to delete task");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to connect to database");
            }

        }






    }
}
