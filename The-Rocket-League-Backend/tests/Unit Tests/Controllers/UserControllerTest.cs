using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using The_Rocket_League_Backend.Controllers;
using The_Rocket_League_Backend.Data;
using The_Rocket_League_Backend.Dtos;
using The_Rocket_League_Backend.Helpers;
using The_Rocket_League_Backend.Models;
using Xunit;

namespace Tests.Unit_Tests.Controllers{
    public class UserControllerTest{
        private static Mock<IUserRepository> UserRepo = new Mock<IUserRepository>();

        private static readonly Mapper Mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<AutoMapperProfiles>();
        }));

        private static readonly User MockUser = new User{
            Id = 1,
            Username = "MockUser"
        };

        private static readonly ClaimsPrincipal MockHttpContextUser = new ClaimsPrincipal(new ClaimsIdentity(new[]{
            new Claim(ClaimTypes.NameIdentifier, MockUser.Id.ToString())
        }));

        private readonly UsersController controller = new UsersController(UserRepo.Object, Mapper){
            ControllerContext = new ControllerContext{
                HttpContext = new DefaultHttpContext{ User = MockHttpContextUser }
            }
        };

        [Fact]
        public async void GetUser_ReturnsBadRequest_IfThereIsNoSuchAUser(){
            UserRepo.Setup(x => x.UserExists(MockUser.Id)).ReturnsAsync(false);

            var result = await controller.GetUser(MockUser.Id);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void GetUser_ReturnsOkResultWithUser_IfThereSuchAUser(){
            UserRepo.Setup(x => x.UserExists(MockUser.Id)).ReturnsAsync(true);
            UserRepo.Setup(x => x.GetUser(MockUser.Id)).ReturnsAsync(MockUser);

            var result = await controller.GetUser(MockUser.Id);
            var resultUser = (result as OkObjectResult)?.Value as UserWithRocketsDto;

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(MockUser.Id, resultUser.Id);
            Assert.Equal(MockUser.Username, resultUser.Username);
        }
    }
}