using Microsoft.EntityFrameworkCore;
using EasyParking.Core.Entities;

namespace EasyParking.Infrastructure.Data
{
    public class EasyParkingDbContext : DbContext
    {
        public EasyParkingDbContext(DbContextOptions<EasyParkingDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Parking> Parkings { get; set; }
        public DbSet<ParkingSpace> ParkingSpaces { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.LicensePlate).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Role).HasConversion<string>();
                
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.LicensePlate).IsUnique();
            });

            // Parking configuration
            modelBuilder.Entity<Parking>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(500);
                entity.Property(e => e.City).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PostalCode).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Latitude).HasPrecision(18, 6);
                entity.Property(e => e.Longitude).HasPrecision(18, 6);
                entity.Property(e => e.HourlyRate).HasPrecision(18, 2);
                entity.Property(e => e.DailyRate).HasPrecision(18, 2);
                entity.Property(e => e.Status).HasConversion<string>();
                
                entity.HasIndex(e => e.City);
                entity.HasIndex(e => e.Status);
            });

            // ParkingSpace configuration
            modelBuilder.Entity<ParkingSpace>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SpaceNumber).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Type).HasConversion<string>();
                entity.Property(e => e.Status).HasConversion<string>();
                entity.Property(e => e.SpecialRate).HasPrecision(18, 2);
                
                entity.HasOne(e => e.Parking)
                    .WithMany(p => p.ParkingSpaces)
                    .HasForeignKey(e => e.ParkingId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasIndex(e => new { e.ParkingId, e.SpaceNumber }).IsUnique();
            });

            // Reservation configuration
            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StartTime).IsRequired();
                entity.Property(e => e.EndTime).IsRequired();
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.Status).HasConversion<string>();
                entity.Property(e => e.PaymentStatus).HasConversion<string>();
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Reservations)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.Parking)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(e => e.ParkingId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.ParkingSpace)
                    .WithMany(ps => ps.Reservations)
                    .HasForeignKey(e => e.ParkingSpaceId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.ParkingId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => new { e.StartTime, e.EndTime });
            });
        }
    }
} 