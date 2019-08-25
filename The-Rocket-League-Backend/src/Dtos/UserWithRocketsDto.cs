namespace The_Rocket_League_Backend.Dtos{
    public class UserWithRocketsDto{
        public int Id{ get; set; }
        public string Username{ get; set; }
        public RocketWithUserDto[] Rockets;
    }
}