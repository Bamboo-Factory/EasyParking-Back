using EasyParking.Application.DTOs;

namespace EasyParking.Application.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationDto>> GetAllReservationsAsync();
        Task<ReservationDto?> GetReservationByIdAsync(int id);
        Task<IEnumerable<ReservationDto>> GetReservationsByUserIdAsync(int userId);
        Task<IEnumerable<ReservationDto>> GetReservationsByParkingIdAsync(int parkingId);
        Task<IEnumerable<ReservationDto>> GetActiveReservationsAsync();
        Task<IEnumerable<ReservationDto>> SearchReservationsAsync(ReservationSearchDto searchDto);
        Task<ReservationDto> CreateReservationAsync(CreateReservationDto createReservationDto);
        Task<ReservationDto> UpdateReservationAsync(int id, UpdateReservationDto updateReservationDto);
        Task DeleteReservationAsync(int id);
        Task<bool> ReservationExistsAsync(int id);
        Task<bool> HasConflictingReservationAsync(int parkingSpaceId, DateTime startTime, DateTime endTime);
        Task<ReservationDto> ConfirmReservationAsync(int id);
        Task<ReservationDto> CancelReservationAsync(int id);
        Task<ReservationDto> CheckInReservationAsync(int id);
        Task<ReservationDto> CheckOutReservationAsync(int id);
    }
} 