using System;

namespace The_Rocket_League_Backend.Dtos{
    public class RocketWithUserDto{
        public int Id{ get; set; }
        public DateTime DateAdded{ get; set; }

        public UserToReturnDto User{ get; set; }
    }
}