using Microsoft.EntityFrameworkCore;
using EasyParking.Core.Interfaces;
using EasyParking.Core.Entities;
using EasyParking.Infrastructure.Data;

namespace EasyParking.Infrastructure.Repositories
{
    public class ParkingSpaceRepository : BaseRepository<ParkingSpace>, IParkingSpaceRepository
    {
        public ParkingSpaceRepository(EasyParkingDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ParkingSpace>> GetByParkingIdAsync(int parkingId)
        {
            return await _dbSet.Where(ps => ps.ParkingId == parkingId && ps.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<ParkingSpace>> GetAvailableSpacesAsync(int parkingId)
        {
            return await _dbSet.Where(ps => ps.ParkingId == parkingId && ps.Status == SpaceStatus.Available && ps.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<ParkingSpace>> GetByTypeAsync(int parkingId, SpaceType type)
        {
            return await _dbSet.Where(ps => ps.ParkingId == parkingId && ps.Type == type && ps.IsActive).ToListAsync();
        }

        public async Task<ParkingSpace?> GetBySpaceNumberAsync(int parkingId, string spaceNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(ps => ps.ParkingId == parkingId && ps.SpaceNumber == spaceNumber && ps.IsActive);
        }

        public async Task UpdateStatusAsync(int spaceId, SpaceStatus status)
        {
            var space = await GetByIdAsync(spaceId);
            if (space != null)
            {
                space.Status = status;
                space.UpdatedAt = DateTime.UtcNow;
                
                _dbSet.Update(space);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetAvailableSpacesCountAsync(int parkingId)
        {
            return await _dbSet.CountAsync(ps => ps.ParkingId == parkingId && ps.Status == SpaceStatus.Available && ps.IsActive);
        }
    }
} 