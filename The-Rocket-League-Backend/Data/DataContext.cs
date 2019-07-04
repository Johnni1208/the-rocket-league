using Microsoft.EntityFrameworkCore;
using The_Rocket_League_Backend.Models;

namespace The_Rocket_League_Backend.Data{
    public class DataContext : DbContext{
        public DataContext(DbContextOptions options) : base(options){ }

        public DbSet<Value> Values{ get; set; }
    }
}