using EasyParking.Core.Entities;

namespace EasyParking.Core.Interfaces
{
    public interface IParkingRepository : IRepository<Parking>
    {
        Task<IEnumerable<Parking>> GetByCityAsync(string city);
        Task<IEnumerable<Parking>> GetByStatusAsync(ParkingStatus status);
        Task<IEnumerable<Parking>> GetAvailableParkingsAsync();
        Task<IEnumerable<Parking>> SearchByLocationAsync(decimal latitude, decimal longitude, double radiusKm);
        Task<IEnumerable<Parking>> SearchByLocationOptimizedAsync(decimal latitude, decimal longitude, double radiusKm);
        Task<Parking?> GetWithSpacesAsync(int id);
        Task UpdateAvailableSpacesAsync(int parkingId, int availableSpaces);
    }
} 