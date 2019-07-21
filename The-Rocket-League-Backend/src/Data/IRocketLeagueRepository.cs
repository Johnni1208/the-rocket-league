using System.Collections.Generic;
using System.Threading.Tasks;
using The_Rocket_League_Backend.Models;

namespace The_Rocket_League_Backend.Data{
    public interface IRocketLeagueRepository{
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<User>> GetUsers();
        Task<IEnumerable<Rocket>> GetRockets();
        Task<User> GetUser(int id);
        Task<Rocket> GetRocket(int id);
    }
}