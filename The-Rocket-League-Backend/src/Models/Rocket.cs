using System;

namespace The_Rocket_League_Backend.Models{
    public class Rocket{
        public int Id{ get; set; }
        public DateTime DateAdded{ get; set; }

        public User User{ get; set; }
        public int UserId{ get; set; }

        public Rocket(){
            DateAdded = DateTime.Now;
        }
    }
}