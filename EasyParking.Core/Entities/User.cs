using System.ComponentModel.DataAnnotations;

namespace EasyParking.Core.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string LicensePlate { get; set; } = string.Empty;
        
        public string? PasswordHash { get; set; }
        
        public UserRole Role { get; set; } = UserRole.Customer;
        
        // Navigation properties
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
    
    public enum UserRole
    {
        Customer,
        Admin,
        ParkingOwner
    }
} 