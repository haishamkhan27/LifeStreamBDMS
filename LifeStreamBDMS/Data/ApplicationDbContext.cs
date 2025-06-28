using Microsoft.EntityFrameworkCore;
using LifeStreamBDMS.Models;

namespace LifeStreamBDMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Donor> Donors { get; set; }
        public DbSet<BloodInventory> BloodInventories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BloodRequest> BloodRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BloodRequest>().ToTable("BloodRequests"); // ✅ Explicit table mapping
        }


    }
}
