using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace TouristApp.Models.DBModel
{
    public class TouristAppDbContext : DbContext
    {
        public TouristAppDbContext(DbContextOptions<TouristAppDbContext> context): base(context)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Hotel> Hotels { get; set; }
    }
}
