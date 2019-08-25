using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using The_Rocket_League_Backend.Data;
using The_Rocket_League_Backend.Dtos;

namespace The_Rocket_League_Backend.Controllers{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase{
        private readonly IUserRepository userRepo;
        private readonly IMapper mapper;

        public UsersController(IUserRepository userRepo, IMapper mapper){
            this.userRepo = userRepo;
            this.mapper = mapper;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUser(int id){
            if (await userRepo.UserExists(id)){
                var userFromRepo = await userRepo.GetUser(id);

                var userToReturn = mapper.Map<UserWithRocketsDto>(userFromRepo);
                return Ok(userToReturn);
            }

            return BadRequest($"No user found under id: {id}");
        }
    }
}