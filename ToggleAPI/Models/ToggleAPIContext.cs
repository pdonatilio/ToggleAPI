using Microsoft.EntityFrameworkCore;

namespace ToggleAPI.Models
{
    public class ToggleAPIContext : DbContext
    {
        public ToggleAPIContext(DbContextOptions<ToggleAPIContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Toggle> Toggles { get; set; }

        public DbSet<TogglesServices> TogglesServices { get; set; }
        
    }
}