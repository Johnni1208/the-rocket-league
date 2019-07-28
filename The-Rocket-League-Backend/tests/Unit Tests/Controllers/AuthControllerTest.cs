using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using The_Rocket_League_Backend.Controllers;
using The_Rocket_League_Backend.Data;
using The_Rocket_League_Backend.Dtos;
using The_Rocket_League_Backend.Helpers;
using The_Rocket_League_Backend.Models;
using Xunit;

namespace Tests.Unit_Tests.Controllers{
    public class AuthControllerTest{
        private readonly UserForRegisterDto mockUserForRegisterDto = new UserForRegisterDto{
            Username = "John",
            Password = "test123"
        };

        private readonly UserForLoginDto mockUserForLoginDto = new UserForLoginDto{
            Username = "John",
            Password = "test123"
        };

        private readonly User mockUserForToken = new User{
            Id = 1,
            Username = "John"
        };

        private static readonly Mock<IAuthRepository> AuthRepo = new Mock<IAuthRepository>();
        private static readonly Mock<IConfiguration> Config = new Mock<IConfiguration>();

        private readonly AuthController controller = new AuthController(AuthRepo.Object, Config.Object);

        [Fact]
        public async void Register_ReturnsRegistersAUser_WhenUserDoesNotExists(){
            AuthRepo.Setup(repo => repo.UserExists(It.IsAny<string>()))
                .ReturnsAsync(false);

            await controller.Register(mockUserForRegisterDto);

            AuthRepo.Verify(x => x.Register(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void Register_ReturnsStatusCode201_WhenUserDoesNotExists(){
            const int statusCodeToReturn = 201;

            AuthRepo.Setup(repo => repo.UserExists(It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await controller.Register(mockUserForRegisterDto);
            var resultStatusCode = (result as StatusCodeResult)?.StatusCode;

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(statusCodeToReturn, resultStatusCode);
        }

        [Fact]
        public async void Register_ReturnsBadRequest_WhenUserExists(){
            AuthRepo.Setup(repo => repo.UserExists(It.IsAny<string>()))
                .ReturnsAsync(true);

            var result = await controller.Register(mockUserForRegisterDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Login_LogsInTheUser_WhenUserCanLogin(){
            await controller.Login(mockUserForLoginDto);
            AuthRepo.Verify(x => x.Login(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void Login_ReturnsUnauthorized_WhenUserCannotLogin(){
            AuthRepo.Setup(repo => repo.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((User) null);

            var result = await controller.Login(mockUserForLoginDto);

            Assert.IsType<UnauthorizedResult>(result);
            AuthRepo.Reset();
        }

        [Fact]
        public async Task Login_ReturnsOkWithToken_WhenUserCanLogin(){
            var tokenHandler = new JwtSecurityTokenHandler();
            AuthRepo.Setup(repo => repo.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(mockUserForToken);
            Config.Setup(config => config.GetSection(It.IsAny<string>()).Value)
                .Returns("therocketleague12082002");

            var result = await controller.Login(mockUserForLoginDto);
            var resultToken = (result as OkObjectResult)?.Value;

            var securityToken = TokenHelper.CreateWritableToken(Config.Object, mockUserForToken.Id, mockUserForToken.Username);
            var writtenToken = new{
                token = tokenHandler.WriteToken(securityToken)
            };

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(writtenToken.ToString(), resultToken?.ToString());
            AuthRepo.Reset();
        }
    }
}