using System.Threading.Tasks;
using The_Rocket_League_Backend.Models;

namespace The_Rocket_League_Backend.Data{
    public interface IUserRepository{
        Task<bool> UserExists(string username);
        Task<bool> UserExists(int id);
        Task<User> GetUser(int id);
    }
}