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
    public class AuthControllerUnitTest{
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

        private readonly Mock<IAuthRepository> authRepo = new Mock<IAuthRepository>();
        private Mock<IConfiguration> config = new Mock<IConfiguration>();

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUserExists(){
            authRepo.Setup(repo => repo.UserExists(It.IsAny<string>()))
                .ReturnsAsync(true);
            var controller = new AuthController(authRepo.Object, config.Object);

            var result = await controller.Register(mockUserForRegisterDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_ReturnsStatusCode201_WhenUserDoesNotExists(){
            authRepo.Setup(repo => repo.UserExists(It.IsAny<string>()))
                .ReturnsAsync(false);
            var controller = new AuthController(authRepo.Object, config.Object);

            var result = await controller.Register(mockUserForRegisterDto);

            Assert.IsType<StatusCodeResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenUserCannotLogin(){
            authRepo.Setup(repo => repo.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((User) null);
            var controller = new AuthController(authRepo.Object, config.Object);

            var result = await controller.Login(mockUserForLoginDto);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenUserCanLogin(){
            authRepo.Setup(repo => repo.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(mockUserForToken);

            config.Setup(config => config.GetSection(It.IsAny<string>()).Value)
                .Returns("therocketleague12082002");

            var controller = new AuthController(authRepo.Object, config.Object);

            var result = await controller.Login(mockUserForLoginDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsOkWithToken_WhenUserCanLogin(){
            var tokenHandler = new JwtSecurityTokenHandler();

            authRepo.Setup(repo => repo.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(mockUserForToken);

            config.Setup(config => config.GetSection(It.IsAny<string>()).Value)
                .Returns("therocketleague12082002");

            var controller = new AuthController(authRepo.Object, config.Object);

            var result = await controller.Login(mockUserForLoginDto);

            var securityToken = TokenHelper.CreateWritableToken(config.Object, mockUserForToken.Id, mockUserForToken.Username);

            var writtenToken = new{
                token = tokenHandler.WriteToken(securityToken)
            };

            var resultToken = (result as OkObjectResult)?.Value;

            Assert.Equal(writtenToken.ToString(), resultToken?.ToString());
        }
    }
}