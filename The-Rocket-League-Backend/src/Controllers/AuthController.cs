using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using The_Rocket_League_Backend.Data;
using The_Rocket_League_Backend.Dtos;
using The_Rocket_League_Backend.Models;

namespace The_Rocket_League_Backend.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase{
        private readonly IAuthRepository repo;

        public AuthController(IAuthRepository repo){
            this.repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto){
            userForRegisterDto.Username = userForRegisterDto.Username.Trim().ToLower();

            if (await repo.UserExists(userForRegisterDto.Username)){
                return BadRequest("Benutzer gibt es bereits.");
            }

            var user = new User{ Username = userForRegisterDto.Username };

            await repo.Register(user, userForRegisterDto.Password);

            return StatusCode(201);
        }
    }
}