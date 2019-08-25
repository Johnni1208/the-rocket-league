using AutoMapper;
using The_Rocket_League_Backend.Dtos;
using The_Rocket_League_Backend.Models;

namespace The_Rocket_League_Backend.Helpers{
    public class AutoMapperProfiles : Profile{
        public AutoMapperProfiles(){
            CreateMap<Rocket, RocketWithUserDto>();
            CreateMap<Rocket, SimpleRocketDto>();
            CreateMap<User, UserToReturnDto>();
            CreateMap<User, UserWithRocketsDto>();
        }
    }
}