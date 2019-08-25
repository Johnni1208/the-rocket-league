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
        private readonly IRocketRepository rocketRepo;
        private readonly IUserRepository userRepo;
        private readonly IMapper mapper;

        public RocketsController(IRocketRepository rocketRepo, IUserRepository userRepo, IMapper mapper){
            this.rocketRepo = rocketRepo;
            this.userRepo = userRepo;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetRockets(){
            var rockets = await rocketRepo.GetRockets();

            var rocketsToReturn = mapper.Map<IEnumerable<RocketWithUserDto>>(rockets);

            return Ok(rocketsToReturn);
        }

        [HttpGet("{id}", Name = "GetRocket")]
        public async Task<IActionResult> GetRocket(int id){
            var rocket = await rocketRepo.GetRocket(id);

            var rocketToReturn = mapper.Map<RocketWithUserDto>(rocket);

            return Ok(rocketToReturn);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddRocket(int userId){
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }

            var userFromRepo = await userRepo.GetUser(userId);

            var rocket = new Rocket();

            userFromRepo.Rockets.Add(rocket);

            if (await rocketRepo.SaveAll()){
                return CreatedAtRoute("GetRocket", new{ id = rocket.Id }, mapper.Map<RocketWithUserDto>(rocket));
            }

            return BadRequest("Could not add a new Rocket");
        }

        [HttpDelete("{userId}/{id}")]
        public async Task<IActionResult> DeleteRocket(int userId, int id){
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }

            var userFromRepo = await userRepo.GetUser(userId);

            if (!userFromRepo.Rockets.Any(r => r.Id == id)){
                return Unauthorized();
            }

            var rocketFromRepo = await rocketRepo.GetRocket(id);

            rocketRepo.Delete(rocketFromRepo);

            if (await rocketRepo.SaveAll()){
                return Ok();
            }

            return BadRequest("Failed to delete the rocket");
        }
    }
}