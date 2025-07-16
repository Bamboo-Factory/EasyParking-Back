using System.ComponentModel.DataAnnotations;

namespace EasyParking.Core.Entities
{
    public class ParkingSpace : BaseEntity
    {
        [Required]
        [StringLength(10)]
        public string SpaceNumber { get; set; } = string.Empty;
        
        public int ParkingId { get; set; }
        
        public SpaceType Type { get; set; } = SpaceType.Standard;
        
        public SpaceStatus Status { get; set; } = SpaceStatus.Available;
        
        public decimal? SpecialRate { get; set; }
        
        public string? Notes { get; set; }
        
        // Navigation properties
        public virtual Parking Parking { get; set; } = null!;
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
    
    public enum SpaceType
    {
        Standard,
        Disabled,
        Electric,
        Motorcycle,
        Large
    }
    
    public enum SpaceStatus
    {
        Available,
        Occupied,
        Reserved,
        Maintenance,
        OutOfService
    }
} 