using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using The_Rocket_League_Backend.Data;
using The_Rocket_League_Backend.Dtos;
using The_Rocket_League_Backend.Helpers;
using The_Rocket_League_Backend.Models;

namespace The_Rocket_League_Backend.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase{
        private readonly IAuthRepository repo;
        private readonly IConfiguration config;

        public AuthController(IAuthRepository repo, IConfiguration config){
            this.repo = repo;
            this.config = config;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto){
            var userFromRepo = await repo.Login(userForLoginDto.Username.Trim().ToLower(), userForLoginDto.Password);

            if (userFromRepo == null){
                return Unauthorized();
            }

            var token = TokenHelper.CreateToken(config, userFromRepo.Id, userFromRepo.Username);

            var tokenHandler = new JwtSecurityTokenHandler();

            return Ok(new{
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}