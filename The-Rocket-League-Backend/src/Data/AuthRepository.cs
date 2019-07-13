using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using The_Rocket_League_Backend.Models;

namespace The_Rocket_League_Backend.Data{
    public class AuthRepository : IAuthRepository{
        private readonly DataContext context;

        public AuthRepository(DataContext context){
            this.context = context;
        }

        public async Task<bool> UserExists(string username){
            if (await context.Users.AnyAsync(x => x.Username == username)){
                return true;
            }

            return false;
        }

        public async Task<User> Register(User user, string password){
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;


            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt){
            using (var hmac = new HMACSHA512()){
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<User> Login(string username, string password){
            var userToLogin = await context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (userToLogin == null){
                return null;
            }

            if (!VerifyPasswordHash(password, userToLogin.PasswordHash, userToLogin.PasswordSalt)){
                return null;
            }

            return userToLogin;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt){
            using (var hmac = new HMACSHA512(passwordSalt)){
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (var i = 0; i < computedHash.Length; i++){
                    if (computedHash[i] != passwordHash[i]) return false;
                }

                return true;
            }
        }
    }
}