using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using The_Rocket_League_Backend.Models;

namespace The_Rocket_League_Backend.Data{
    public class UserRepository: IUserRepository{
        private readonly DataContext context;

        public UserRepository(DataContext context){
            this.context = context;
        }

        public async Task<bool> UserExists(string username){
            return await context.Users.AnyAsync(x => x.Username == username);
        }

        public async Task<bool> UserExists(int id){
            return await context.Users.AnyAsync(x => x.Id == id);
        }

        public async Task<User> GetUser(int id){
            var user = await context.Users.Include(r => r.Rockets)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }
    }
}