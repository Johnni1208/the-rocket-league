using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using The_Rocket_League_Backend.Models;

namespace The_Rocket_League_Backend.Data{
    public class RocketLeagueRepository : IRocketLeagueRepository{
        private readonly DataContext context;

        public RocketLeagueRepository(DataContext context){
            this.context = context;
        }

        public void Add<T>(T entity) where T : class{
            context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class{
            context.Remove(entity);
        }

        public async Task<bool> SaveAll(){
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<User>> GetUsers(){
            var users = await context.Users.Include(r => r.Rockets).ToListAsync();

            return users;
        }

        public async Task<IEnumerable<Rocket>> GetRockets(){
            var rockets = await context.Rockets.Include(u => u.User).ToListAsync();

            return rockets;
        }

        public async Task<User> GetUser(int id){
            var user = await context.Users.Include(r => r.Rockets)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<Rocket> GetRocket(int id){
            var rocket = await context.Rockets.Include(u => u.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            return rocket;
        }
    }
}