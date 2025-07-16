namespace EasyParking.Application.DTOs
{
    public class ParkingDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int TotalSpaces { get; set; }
        public int AvailableSpaces { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal DailyRate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateParkingDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int TotalSpaces { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal DailyRate { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class UpdateParkingDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal DailyRate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class ParkingSearchDto
    {
        public string? City { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public double? RadiusKm { get; set; }
        public string? Status { get; set; }
    }
} 