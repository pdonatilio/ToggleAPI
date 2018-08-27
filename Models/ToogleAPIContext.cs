using Microsoft.EntityFrameworkCore;

namespace ToggleAPI.Models
{
    public class ToogleAPIContext : DbContext
    {
        public ToogleAPIContext(DbContextOptions<ToogleAPIContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        
    }
}