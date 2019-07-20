using System.Collections.Generic;

namespace The_Rocket_League_Backend.Models{
    public class User{
        public int Id{ get; set; }
        public string Username{ get; set; }
        public byte[] PasswordHash{ get; set; }
        public byte[] PasswordSalt{ get; set; }
        public ICollection<Rocket> Rockets{ get; set; }
    }
}