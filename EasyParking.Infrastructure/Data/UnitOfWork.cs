using Microsoft.EntityFrameworkCore.Storage;
using EasyParking.Core.Interfaces;
using EasyParking.Infrastructure.Repositories;

namespace EasyParking.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EasyParkingDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(EasyParkingDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users => new UserRepository(_context);
        public IParkingRepository Parkings => new ParkingRepository(_context);
        public IParkingSpaceRepository ParkingSpaces => new ParkingSpaceRepository(_context);
        public IReservationRepository Reservations => new ReservationRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
} 