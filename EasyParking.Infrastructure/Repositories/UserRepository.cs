using Microsoft.EntityFrameworkCore;
using EasyParking.Core.Interfaces;
using EasyParking.Core.Entities;
using EasyParking.Infrastructure.Data;

namespace EasyParking.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(EasyParkingDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public async Task<User?> GetByLicensePlateAsync(string licensePlate)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.LicensePlate == licensePlate && u.IsActive);
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
        {
            return await _dbSet.Where(u => u.Role == role && u.IsActive).ToListAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email && u.IsActive);
        }

        public async Task<bool> LicensePlateExistsAsync(string licensePlate)
        {
            return await _dbSet.AnyAsync(u => u.LicensePlate == licensePlate && u.IsActive);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }
    }
} 