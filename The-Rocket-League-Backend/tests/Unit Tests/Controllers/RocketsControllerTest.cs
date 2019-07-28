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
        private readonly Mock<IRocketLeagueRepository> repo = new Mock<IRocketLeagueRepository>();

        private static readonly MapperConfiguration MapperConfig = new MapperConfiguration(cfg => {
            cfg.AddProfile<AutoMapperProfiles>();
        });

        private readonly Mapper mapper = new Mapper(MapperConfig);

        private RocketsController GetController(){
            var controller = new RocketsController(repo.Object, mapper){
                ControllerContext = new ControllerContext{
                    HttpContext = new DefaultHttpContext{ User = mockHttpContextUser }
                }
            };

            return controller;
        }

        private readonly ClaimsPrincipal mockHttpContextUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]{
            new Claim(ClaimTypes.NameIdentifier, MockUser.Id.ToString())
        }));

        private static readonly User MockUser = new User{
            Id = 1,
            Rockets = new List<Rocket>()
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
            repo.Setup(x => x.GetRockets()).ReturnsAsync(GetFakeListOfRockets());

            var controller = GetController();
            var result = await controller.GetRockets();
            var resultRockets = (result as OkObjectResult)?.Value as List<RocketForListDto>;

            var count = resultRockets?.Count;

            Assert.Equal(5, count);
        }

        [Fact]
        public async void GetRockets_Returns0Rockets_IfThereAreNoRocketsInTheDatabase(){
            repo.Setup(x => x.GetRockets()).ReturnsAsync(Enumerable.Empty<Rocket>());

            var controller = GetController();
            var result = await controller.GetRockets();
            var resultRockets = (result as OkObjectResult)?.Value as List<RocketForListDto>;

            var count = resultRockets?.Count;

            Assert.Equal(0, count);
        }

        [Fact]
        public async void GetRocket_ReturnsOneSpecificRocket(){
            repo.Setup(x => x.GetRocket(It.IsAny<int>())).ReturnsAsync(mockRocket);

            var controller = GetController();

            var result = await controller.GetRocket(It.IsAny<int>());
            var resultRocket = (result as OkObjectResult)?.Value as RocketToReturn;

            Assert.Equal(mockRocket.Id, resultRocket?.Id);
            Assert.Equal(mockRocket.DateAdded, resultRocket?.DateAdded);
            Assert.Equal(It.IsAny<UserToReturnDto>(), resultRocket?.User);
        }

        [Fact]
        public async void GetRocket_ReturnsNoRocket_IfThereIsNotAnyMatchingRocket(){
            repo.Setup(x => x.GetRocket(It.IsAny<int>())).ReturnsAsync(null as Rocket);

            var controller = GetController();

            var result = await controller.GetRocket(It.IsAny<int>());
            var resultRocket = (result as OkObjectResult)?.Value;

            Assert.IsType<OkObjectResult>(result);
            Assert.Null(resultRocket);
        }

        [Fact]
        public async void AddRocket_AddsARocketToTheDatabase(){
            repo.Setup(x => x.GetUser(It.IsAny<int>()))
                .ReturnsAsync(MockUser);
            repo.Setup(x => x.SaveAll()).ReturnsAsync(true);
            var controller = GetController();

            await controller.AddRocket(MockUser.Id);

            Assert.True(MockUser.Rockets.Count == 1);
            MockUser.Rockets.Clear();
        }

        [Fact]
        public async void DeleteRocket_DeletesRocket_IfAnyExists(){
            MockUser.Rockets.Add(mockRocket);

            repo.Setup(x => x.GetUser(It.IsAny<int>()))
                .ReturnsAsync(MockUser);
            repo.Setup(x => x.GetRocket(It.IsAny<int>()))
                .ReturnsAsync(mockRocket);

            var controller = GetController();
            await controller.DeleteRocket(MockUser.Id, mockRocket.Id);

            repo.Verify(x => x.Delete(mockRocket), Times.Once);
            MockUser.Rockets.Clear();
        }

        [Fact]
        public async void DeleteRocket_ReturnsUnauthorized_IfNoRocketHasMatchingId(){
            repo.Setup(x => x.GetUser(It.IsAny<int>()))
                .ReturnsAsync(MockUser);

            var controller = GetController();
            var result = await controller.DeleteRocket(MockUser.Id, 1);

            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}