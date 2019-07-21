using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GenFu;
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
        private User mockUserToReturn = new User{
            Id = 1,
            Username = "mockUser"
        };

        private IEnumerable<Rocket> GetFakeData(){
            var i = 1;
            var rockets = A.ListOf<Rocket>(5);
            rockets.ForEach(x => x.Id = i++);
            rockets.ForEach(x => x.DateAdded = DateTime.Now);
            rockets.ForEach(x => x.User = mockUserToReturn);
            return rockets.ToList();
        }

        [Fact]
        public async void GetRockets_ReturnsAListOfRocketForListDtos(){
            var service = new Mock<IRocketLeagueRepository>();

            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfiles>();
            });

            var mapper = new Mapper(mapperConfig);

            var rockets = GetFakeData();

            service.Setup(x => x.GetRockets()).Returns(Task.FromResult(rockets));

            var controller = new RocketsController(service.Object, mapper);

            var result = await controller.GetRockets();

            var resultRockets = (result as OkObjectResult)?.Value as List<RocketForListDto>;

            var count = resultRockets?.Count;

            Assert.Equal(5, count);
        }
    }
}