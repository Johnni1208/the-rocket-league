using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using The_Rocket_League_Backend.Controllers;
using The_Rocket_League_Backend.Data;
using The_Rocket_League_Backend.Dtos;
using Xunit;

namespace Tests.Unit_Tests.Controllers{
    public class AuthControllerUnitTest{
        private UserForRegisterDto mockUserForRegisterDto = new UserForRegisterDto{
            Username = "John",
            Password = "test123"
        };

        private Mock<IAuthRepository> authRepo = new Mock<IAuthRepository>();

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUserExists(){
            authRepo.Setup(repo => repo.UserExists(It.IsAny<string>()))
                .ReturnsAsync(true);
            var controller = new AuthController(authRepo.Object);

            var result = await controller.Register(mockUserForRegisterDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_ReturnsStatusCode201_WhenUserDoesNotExists(){
            authRepo.Setup(repo => repo.UserExists(It.IsAny<string>()))
                .ReturnsAsync(false);
            var controller = new AuthController(authRepo.Object);

            var result = await controller.Register(mockUserForRegisterDto);

            Assert.IsType<StatusCodeResult>(result);
        }
    }
}