using Microsoft.EntityFrameworkCore;
using The_Rocket_League_Backend.Models;

namespace The_Rocket_League_Backend.Data{
    public class DataContext : DbContext{
        public DataContext(DbContextOptions options) : base(options){ }

        public DbSet<Value> Values{ get; set; }

        public DbSet<User> Users{ get; set; }

        public DbSet<Rocket> Rockets{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<Rocket>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rockets)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}