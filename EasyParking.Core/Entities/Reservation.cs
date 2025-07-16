using System.ComponentModel.DataAnnotations;

namespace EasyParking.Core.Entities
{
    public class Reservation : BaseEntity
    {
        public int UserId { get; set; }
        
        public int ParkingId { get; set; }
        
        public int ParkingSpaceId { get; set; }
        
        [Required]
        public DateTime StartTime { get; set; }
        
        [Required]
        public DateTime EndTime { get; set; }
        
        public decimal TotalAmount { get; set; }
        
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        
        public string? Notes { get; set; }
        
        public DateTime? CheckInTime { get; set; }
        
        public DateTime? CheckOutTime { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Parking Parking { get; set; } = null!;
        public virtual ParkingSpace ParkingSpace { get; set; } = null!;
    }
    
    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        Active,
        Completed,
        Cancelled,
        Expired
    }
    
    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed,
        Refunded
    }
} 