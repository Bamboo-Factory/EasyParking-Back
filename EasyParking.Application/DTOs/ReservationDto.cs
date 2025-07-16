namespace EasyParking.Application.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ParkingId { get; set; }
        public int ParkingSpaceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public UserDto? User { get; set; }
        public ParkingDto? Parking { get; set; }
        public ParkingSpaceDto? ParkingSpace { get; set; }
    }

    public class CreateReservationDto
    {
        public int UserId { get; set; }
        public int ParkingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class UpdateReservationDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Notes { get; set; }
    }

    public class ReservationSearchDto
    {
        public int? UserId { get; set; }
        public int? ParkingId { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
} 