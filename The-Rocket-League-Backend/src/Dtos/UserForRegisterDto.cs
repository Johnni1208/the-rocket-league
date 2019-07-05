using System.ComponentModel.DataAnnotations;

namespace The_Rocket_League_Backend.Dtos{
    public class UserForRegisterDto{
        [Required] 
        public string Username{ get; set; }
        
        [Required]
        [MinLength(2, ErrorMessage = "Dein Passwort muss mindestens 2 Zeichen haben.")]
        public string Password{ get; set; }
    }
}