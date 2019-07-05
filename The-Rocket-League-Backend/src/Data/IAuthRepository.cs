using System.Threading.Tasks;
using The_Rocket_League_Backend.Models;

namespace The_Rocket_League_Backend.Data{
    public interface IAuthRepository{
        Task<bool> UserExists(string username);
        Task<User> Register(User user, string password);
        Task<User> Login(User user, string password);
    }
}