namespace EasyParking.Application.DTOs
{
    public class ParkingSpaceDto
    {
        public int Id { get; set; }
        public string SpaceNumber { get; set; } = string.Empty;
        public int ParkingId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal? SpecialRate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateParkingSpaceDto
    {
        public string SpaceNumber { get; set; } = string.Empty;
        public int ParkingId { get; set; }
        public string Type { get; set; } = "Standard";
        public decimal? SpecialRate { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateParkingSpaceDto
    {
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal? SpecialRate { get; set; }
        public string? Notes { get; set; }
    }
} 