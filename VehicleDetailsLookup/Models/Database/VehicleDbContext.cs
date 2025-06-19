using Microsoft.EntityFrameworkCore;
using VehicleDetailsLookup.Models.Database.Ai;
using VehicleDetailsLookup.Models.Database.Image;
using VehicleDetailsLookup.Models.Database.Details;
using VehicleDetailsLookup.Models.Database.Mot;

namespace VehicleDetailsLookup.Models.Database
{
    public class VehicleDbContext : DbContext
    {
        public VehicleDbContext(DbContextOptions<VehicleDbContext> options) : base(options)
        {
        }

        public DbSet<AiDbModel> AiData { get; set; }
        public DbSet<DetailsDbModel> Details { get; set; }
        public DbSet<ImageDbModel> Images { get; set; }
        public DbSet<MotTestDbModel> MotTests { get; set; }
        public DbSet<MotDefectDbModel> MotDefects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite key for AiDbModel
            modelBuilder.Entity<AiDbModel>()
                .HasKey(a => new { a.RegistrationNumber, a.Type });

            // RegistrationNumber as key for DetailsDbModel
            modelBuilder.Entity<DetailsDbModel>()
                .HasKey(d => d.RegistrationNumber);

            // Composite key for ImageDbModel
            modelBuilder.Entity<ImageDbModel>()
                .HasKey(i => new { i.RegistrationNumber, i.Url });

            // Composite key for MotDefectDbModel
            modelBuilder.Entity<MotDefectDbModel>()
                .HasKey(m => new { m.TestNumber, m.Description });

            // TestNumber as key for MotTestDbModel
            modelBuilder.Entity<MotTestDbModel>()
                .HasKey(m => m.TestNumber);

            // One-to-many: MotTestDbModel -> MotDefectDbModel via TestNumber
            modelBuilder.Entity<MotTestDbModel>()
                .HasMany(t => t.MotDefects)
                .WithOne(d => d.MotTest)
                .HasForeignKey(d => d.TestNumber)
                .HasPrincipalKey(t => t.TestNumber);
        }
    }
}