using System.ComponentModel.DataAnnotations;

namespace EasyParking.Core.Entities
{
    public class Parking : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;
        
        [Required]
        [StringLength(10)]
        public string PostalCode { get; set; } = string.Empty;
        
        public decimal Latitude { get; set; }
        
        public decimal Longitude { get; set; }
        
        public int TotalSpaces { get; set; }
        
        public int AvailableSpaces { get; set; }
        
        public decimal HourlyRate { get; set; }
        
        public decimal DailyRate { get; set; }
        
        public ParkingStatus Status { get; set; } = ParkingStatus.Active;
        
        public string? Description { get; set; }
        
        public string? ImageUrl { get; set; }
        
        // Navigation properties
        public virtual ICollection<ParkingSpace> ParkingSpaces { get; set; } = new List<ParkingSpace>();
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
    
    public enum ParkingStatus
    {
        Active,
        Inactive,
        Maintenance
    }
} 