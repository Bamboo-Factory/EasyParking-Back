namespace EasyParking.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IParkingRepository Parkings { get; }
        IParkingSpaceRepository ParkingSpaces { get; }
        IReservationRepository Reservations { get; }
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
} 