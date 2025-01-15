    using FMISaliAPI.Models;
    using Microsoft.EntityFrameworkCore;

    namespace FMISaliAPI.Data
    {
        public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options) 
        {
            public DbSet<Room> Rooms { get; set; }

            public DbSet<Facility> Facilities { get; set; }

            public DbSet<RoomFacility> RoomFacilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                modelBuilder.Entity<Room>()
                    .Property(r => r.Type)
                    .HasConversion(
                        v => v.ToString(),
                        v => Enum.Parse<RoomType>(v));

                modelBuilder.Entity<Facility>()
                    .Property(r => r.Type)
                    .HasConversion(
                        v => v.ToString(),
                        v => Enum.Parse<FacilityType>(v));

                modelBuilder.Entity<RoomFacility>()
                    .HasKey(rf => new { rf.RoomId, rf.FacilityId });
            }
        }
    }
