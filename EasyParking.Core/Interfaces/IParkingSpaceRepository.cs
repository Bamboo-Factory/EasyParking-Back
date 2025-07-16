using EasyParking.Core.Entities;

namespace EasyParking.Core.Interfaces
{
    public interface IParkingSpaceRepository : IRepository<ParkingSpace>
    {
        Task<IEnumerable<ParkingSpace>> GetByParkingIdAsync(int parkingId);
        Task<IEnumerable<ParkingSpace>> GetAvailableSpacesAsync(int parkingId);
        Task<IEnumerable<ParkingSpace>> GetByTypeAsync(int parkingId, SpaceType type);
        Task<ParkingSpace?> GetBySpaceNumberAsync(int parkingId, string spaceNumber);
        Task UpdateStatusAsync(int spaceId, SpaceStatus status);
        Task<int> GetAvailableSpacesCountAsync(int parkingId);
    }
} 