using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
    public class MarkerContext : DbContext
    {
        public DbSet<Wallet> Wallets { get; set; }

        public MarkerContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Market;Username=postgres;Password=Qwerty1");
        }
    }
}