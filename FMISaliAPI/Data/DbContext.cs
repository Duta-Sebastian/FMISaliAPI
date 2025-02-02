using FMISaliAPI.Models;
using Microsoft.EntityFrameworkCore;
using File = FMISaliAPI.Models.File;

namespace FMISaliAPI.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Room> Rooms { get; set; }

        public DbSet<Facility> Facilities { get; set; }

        public DbSet<RoomFacility> RoomFacilities { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<File> Files { get; set; }


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

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Room)
                .WithMany()
                .HasForeignKey(s => s.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Schedule>()
                .Property(s => s.Status)
                .HasConversion(
                    s => s.ToString(),
                    s => Enum.Parse<Status>(s));

            modelBuilder.Entity<Schedule>()
                .Property(s => s.Type)
                .HasConversion(
                    t => t.ToString(),
                    t => Enum.Parse<ScheduleType>(t));

            modelBuilder.Entity<File>()
                .Property(f => f.FileData)
                .HasColumnType("bytea");

            modelBuilder.Entity<File>()
                .Property(f => f.UploadedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

        }
    }
}
