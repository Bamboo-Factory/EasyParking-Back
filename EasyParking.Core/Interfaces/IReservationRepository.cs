using EasyParking.Core.Entities;

namespace EasyParking.Core.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Reservation>> GetByParkingIdAsync(int parkingId);
        Task<IEnumerable<Reservation>> GetByStatusAsync(ReservationStatus status);
        Task<IEnumerable<Reservation>> GetActiveReservationsAsync();
        Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Reservation>> GetAllWithDetailsAsync();
        Task<Reservation?> GetWithDetailsAsync(int id);
        Task<bool> HasConflictingReservationAsync(int parkingSpaceId, DateTime startTime, DateTime endTime);
        Task UpdateStatusAsync(int reservationId, ReservationStatus status);
        Task UpdatePaymentStatusAsync(int reservationId, PaymentStatus paymentStatus);
    }
} 