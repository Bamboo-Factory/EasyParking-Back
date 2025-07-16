using EasyParking.Core.Entities;

namespace EasyParking.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByLicensePlateAsync(string licensePlate);
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> LicensePlateExistsAsync(string licensePlate);
    }
} 