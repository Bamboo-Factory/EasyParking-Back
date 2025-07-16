using EasyParking.Application.DTOs;

namespace EasyParking.Application.Interfaces
{
    public interface IParkingService
    {
        Task<IEnumerable<ParkingDto>> GetAllAsync();
        Task<ParkingDto?> GetByIdAsync(int id);
        Task<IEnumerable<ParkingDto>> GetByCityAsync(string city);
        Task<IEnumerable<ParkingDto>> GetAvailableParkingsAsync();
        Task<IEnumerable<ParkingDto>> SearchByLocationAsync(decimal latitude, decimal longitude, double radiusKm);
        Task<IEnumerable<ParkingDto>> SearchByLocationOptimizedAsync(decimal latitude, decimal longitude, double radiusKm);
        Task<ParkingDto> CreateAsync(ParkingDto parkingDto);
        Task<ParkingDto> UpdateAsync(int id, ParkingDto parkingDto);
        Task DeleteAsync(int id);
        Task<bool> ParkingExistsAsync(int id);
        Task UpdateAvailableSpacesAsync(int parkingId, int availableSpaces);
    }
} 