using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using The_Rocket_League_Backend.Data;
using The_Rocket_League_Backend.Dtos;
using The_Rocket_League_Backend.Models;

namespace The_Rocket_League_Backend.Controllers{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RocketsController : ControllerBase{
        private readonly IRocketLeagueRepository repo;
        private readonly IMapper mapper;

        public RocketsController(IRocketLeagueRepository repo, IMapper mapper){
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetRockets(){
            var rockets = await repo.GetRockets();

            var rocketsToReturn = mapper.Map<IEnumerable<RocketForListDto>>(rockets);

            return Ok(rocketsToReturn);
        }

        [HttpGet("{id}", Name = "GetRocket")]
        public async Task<IActionResult> GetRocket(int id){
            var rocket = await repo.GetRocket(id);

            var rocketToReturn = mapper.Map<RocketToReturn>(rocket);

            return Ok(rocketToReturn);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddRocket(int userId){
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }

            var userFromRepo = await repo.GetUser(userId);

            var rocket = new Rocket();

            userFromRepo.Rockets.Add(rocket);

            if (await repo.SaveAll()){
                return CreatedAtRoute("GetRocket", new{ id = rocket.Id }, mapper.Map<RocketToReturn>(rocket));
            }

            return BadRequest("Could not add a new Rocket");
        }

        [HttpDelete("{userId}/{id}")]
        public async Task<IActionResult> DeleteRocket(int userId, int id){
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }

            var userFromRepo = await repo.GetUser(userId);

            if (!userFromRepo.Rockets.Any(r => r.Id == id)){
                return Unauthorized();
            }

            var rocketFromRepo = await repo.GetRocket(id);

            repo.Delete(rocketFromRepo);

            if (await repo.SaveAll()){
                return Ok();
            }

            return BadRequest("Failed to delete the rocket");
        }
    }
}