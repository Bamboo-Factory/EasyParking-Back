using Microsoft.EntityFrameworkCore;
using EasyParking.Core.Interfaces;
using EasyParking.Core.Entities;
using EasyParking.Infrastructure.Data;

namespace EasyParking.Infrastructure.Repositories
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(EasyParkingDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Reservation>> GetByUserIdAsync(int userId)
        {
            return await _dbSet.Where(r => r.UserId == userId && r.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByParkingIdAsync(int parkingId)
        {
            return await _dbSet.Where(r => r.ParkingId == parkingId && r.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByStatusAsync(ReservationStatus status)
        {
            return await _dbSet.Where(r => r.Status == status && r.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetActiveReservationsAsync()
        {
            return await _dbSet.Where(r => (r.Status == ReservationStatus.Confirmed || r.Status == ReservationStatus.Active) && r.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(r => 
                (r.StartTime >= startDate && r.StartTime <= endDate) || 
                (r.EndTime >= startDate && r.EndTime <= endDate) && r.IsActive).ToListAsync();
        }

        public async Task<Reservation?> GetWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(r => r.User)
                .Include(r => r.Parking)
                .Include(r => r.ParkingSpace)
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
        }

        public async Task<bool> HasConflictingReservationAsync(int parkingSpaceId, DateTime startTime, DateTime endTime)
        {
            return await _dbSet.AnyAsync(r => 
                r.ParkingSpaceId == parkingSpaceId && 
                r.Status != ReservationStatus.Cancelled && 
                r.Status != ReservationStatus.Completed &&
                r.IsActive &&
                ((r.StartTime <= startTime && r.EndTime > startTime) || 
                 (r.StartTime < endTime && r.EndTime >= endTime) || 
                 (r.StartTime >= startTime && r.EndTime <= endTime)));
        }

        public async Task UpdateStatusAsync(int reservationId, ReservationStatus status)
        {
            var reservation = await GetByIdAsync(reservationId);
            if (reservation != null)
            {
                reservation.Status = status;
                reservation.UpdatedAt = DateTime.UtcNow;
                
                _dbSet.Update(reservation);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePaymentStatusAsync(int reservationId, PaymentStatus paymentStatus)
        {
            var reservation = await GetByIdAsync(reservationId);
            if (reservation != null)
            {
                reservation.PaymentStatus = paymentStatus;
                reservation.UpdatedAt = DateTime.UtcNow;
                
                _dbSet.Update(reservation);
                await _context.SaveChangesAsync();
            }
        }
    }
} 