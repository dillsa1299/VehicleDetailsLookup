using Microsoft.EntityFrameworkCore;
using VehicleDetailsLookup.Models.Database.AiData;
using VehicleDetailsLookup.Models.Database.Image;
using VehicleDetailsLookup.Models.Database.Details;
using VehicleDetailsLookup.Models.Database.Mot;
using VehicleDetailsLookup.Models.Database.Lookup;

namespace VehicleDetailsLookup.Models.Database
{
    public class VehicleDbContext(DbContextOptions<VehicleDbContext> options) : DbContext(options)
    {
        public DbSet<AiDataDbModel> AiData { get; set; }
        public DbSet<DetailsDbModel> Details { get; set; }
        public DbSet<ImageDbModel> Images { get; set; }
        public DbSet<MotTestDbModel> MotTests { get; set; }
        public DbSet<MotDefectDbModel> MotDefects { get; set; }
        public DbSet<LookupDbModel> Lookups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite key for AiDbModel (remove old key, add unique index)
            modelBuilder.Entity<AiDataDbModel>()
                .HasKey(a => new { a.RegistrationNumber, a.Type, a.MetaData });
            modelBuilder.Entity<AiDataDbModel>()
                .HasIndex(a => new { a.RegistrationNumber, a.Type, a.MetaData })
                .IsUnique();

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

            // Composite key for LookupDbModel
            modelBuilder.Entity<LookupDbModel>()
                .HasKey(l => new { l.DateTime, l.RegistrationNumber });

            // One-to-many: DetailsDbModel <-> LookupDbModel via RegistrationNumber
            modelBuilder.Entity<DetailsDbModel>()
                .HasMany(d => d.Lookups)
                .WithOne(l => l.Details)
                .HasForeignKey(l => l.RegistrationNumber)
                .HasPrincipalKey(d => d.RegistrationNumber);
        }
    }
}