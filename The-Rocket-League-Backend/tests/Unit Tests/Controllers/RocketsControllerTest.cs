using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using GenFu;
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
    public class RocketsControllerTest{
        private static readonly Mock<IRocketLeagueRepository> Repo = new Mock<IRocketLeagueRepository>();

        private static readonly Mapper Mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.AddProfile<AutoMapperProfiles>();
        }));

        private static readonly User MockUser = new User{
            Id = 1,
            Rockets = new List<Rocket>()
        };

        private static readonly ClaimsPrincipal MockHttpContextUser = new ClaimsPrincipal(new ClaimsIdentity(new[]{
            new Claim(ClaimTypes.NameIdentifier, MockUser.Id.ToString())
        }));

        private readonly RocketsController controller = new RocketsController(Repo.Object, Mapper){
            ControllerContext = new ControllerContext{
                HttpContext = new DefaultHttpContext{ User = MockHttpContextUser }
            }
        };

        private readonly Rocket mockRocket = new Rocket{
            DateAdded = DateTime.Now,
            Id = 1,
            User = It.IsAny<User>(),
            UserId = 1
        };

        private static IEnumerable<Rocket> GetFakeListOfRockets(){
            var i = 1;
            var rockets = A.ListOf<Rocket>(5);
            rockets.ForEach(x => x.Id = i++);
            rockets.ForEach(x => x.DateAdded = DateTime.Now);
            rockets.ForEach(x => x.User = It.IsAny<User>());
            return rockets.ToList();
        }

        [Fact]
        public async void GetRockets_ReturnsAListOfRocketForListDtos(){
            Repo.Setup(x => x.GetRockets()).ReturnsAsync(GetFakeListOfRockets());

            var result = await controller.GetRockets();
            var resultRockets = (result as OkObjectResult)?.Value as List<RocketForListDto>;

            var count = resultRockets?.Count;

            Assert.Equal(5, count);
        }

        [Fact]
        public async void GetRockets_Returns0Rockets_IfThereAreNoRocketsInTheDatabase(){
            Repo.Setup(x => x.GetRockets()).ReturnsAsync(Enumerable.Empty<Rocket>());

            var result = await controller.GetRockets();
            var resultRockets = (result as OkObjectResult)?.Value as List<RocketForListDto>;

            var count = resultRockets?.Count;

            Assert.Equal(0, count);
        }

        [Fact]
        public async void GetRocket_ReturnsOneSpecificRocket(){
            Repo.Setup(x => x.GetRocket(It.IsAny<int>())).ReturnsAsync(mockRocket);

            var result = await controller.GetRocket(It.IsAny<int>());
            var resultRocket = (result as OkObjectResult)?.Value as RocketToReturn;

            Assert.Equal(mockRocket.Id, resultRocket?.Id);
            Assert.Equal(mockRocket.DateAdded, resultRocket?.DateAdded);
            Assert.Equal(It.IsAny<UserToReturnDto>(), resultRocket?.User);
        }

        [Fact]
        public async void GetRocket_ReturnsNoRocket_IfThereIsNotAnyMatchingRocket(){
            Repo.Setup(x => x.GetRocket(It.IsAny<int>())).ReturnsAsync(null as Rocket);

            var result = await controller.GetRocket(It.IsAny<int>());
            var resultRocket = (result as OkObjectResult)?.Value;

            Assert.IsType<OkObjectResult>(result);
            Assert.Null(resultRocket);
        }

        [Fact]
        public async void AddRocket_AddsARocketToTheDatabase(){
            Repo.Setup(x => x.GetUser(It.IsAny<int>()))
                .ReturnsAsync(MockUser);
            Repo.Setup(x => x.SaveAll()).ReturnsAsync(true);

            await controller.AddRocket(MockUser.Id);

            Assert.True(MockUser.Rockets.Count == 1);
            MockUser.Rockets.Clear();
        }

        [Fact]
        public async void DeleteRocket_DeletesRocket_IfAnyExists(){
            MockUser.Rockets.Add(mockRocket);

            Repo.Setup(x => x.GetUser(It.IsAny<int>()))
                .ReturnsAsync(MockUser);
            Repo.Setup(x => x.GetRocket(It.IsAny<int>()))
                .ReturnsAsync(mockRocket);

            await controller.DeleteRocket(MockUser.Id, mockRocket.Id);

            Repo.Verify(x => x.Delete(mockRocket), Times.Once);
            MockUser.Rockets.Clear();
        }

        [Fact]
        public async void DeleteRocket_ReturnsUnauthorized_IfNoRocketHasMatchingId(){
            Repo.Setup(x => x.GetUser(It.IsAny<int>()))
                .ReturnsAsync(MockUser);

            var result = await controller.DeleteRocket(MockUser.Id, 1);

            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}