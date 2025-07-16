using EasyParking.Application.DTOs;
using EasyParking.Application.Interfaces;
using EasyParking.Core.Interfaces;
using EasyParking.Core.Entities;
using EasyParking.Core.Exceptions;

namespace EasyParking.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IParkingRepository _parkingRepository;
        private readonly IParkingSpaceRepository _parkingSpaceRepository;
        private readonly IUserRepository _userRepository;

        public ReservationService(
            IReservationRepository reservationRepository,
            IParkingRepository parkingRepository,
            IParkingSpaceRepository parkingSpaceRepository,
            IUserRepository userRepository)
        {
            _reservationRepository = reservationRepository;
            _parkingRepository = parkingRepository;
            _parkingSpaceRepository = parkingSpaceRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
        {
            var reservations = await _reservationRepository.GetAllAsync();
            return reservations.Select(MapToDto);
        }

        public async Task<ReservationDto?> GetReservationByIdAsync(int id)
        {
            var reservation = await _reservationRepository.GetWithDetailsAsync(id);
            return reservation != null ? MapToDto(reservation) : null;
        }

        public async Task<IEnumerable<ReservationDto>> GetReservationsByUserIdAsync(int userId)
        {
            var reservations = await _reservationRepository.GetByUserIdAsync(userId);
            return reservations.Select(MapToDto);
        }

        public async Task<IEnumerable<ReservationDto>> GetReservationsByParkingIdAsync(int parkingId)
        {
            var reservations = await _reservationRepository.GetByParkingIdAsync(parkingId);
            return reservations.Select(MapToDto);
        }

        public async Task<IEnumerable<ReservationDto>> GetActiveReservationsAsync()
        {
            var reservations = await _reservationRepository.GetActiveReservationsAsync();
            return reservations.Select(MapToDto);
        }

        public async Task<IEnumerable<ReservationDto>> SearchReservationsAsync(ReservationSearchDto searchDto)
        {
            IEnumerable<Reservation> reservations;

            if (searchDto.UserId.HasValue)
            {
                reservations = await _reservationRepository.GetByUserIdAsync(searchDto.UserId.Value);
            }
            else if (searchDto.ParkingId.HasValue)
            {
                reservations = await _reservationRepository.GetByParkingIdAsync(searchDto.ParkingId.Value);
            }
            else if (!string.IsNullOrEmpty(searchDto.Status))
            {
                var status = Enum.Parse<ReservationStatus>(searchDto.Status);
                reservations = await _reservationRepository.GetByStatusAsync(status);
            }
            else if (searchDto.StartDate.HasValue && searchDto.EndDate.HasValue)
            {
                reservations = await _reservationRepository.GetByDateRangeAsync(searchDto.StartDate.Value, searchDto.EndDate.Value);
            }
            else
            {
                reservations = await _reservationRepository.GetAllAsync();
            }

            return reservations.Select(MapToDto);
        }

        public async Task<ReservationDto> CreateReservationAsync(CreateReservationDto createReservationDto)
        {
            // Validar que el usuario existe
            var user = await _userRepository.GetByIdAsync(createReservationDto.UserId);
            if (user == null)
            {
                throw new NotFoundException("Usuario", createReservationDto.UserId);
            }

            // Validar que el estacionamiento existe
            var parking = await _parkingRepository.GetByIdAsync(createReservationDto.ParkingId);
            if (parking == null)
            {
                throw new NotFoundException("Estacionamiento", createReservationDto.ParkingId);
            }

            // Buscar un espacio disponible en el estacionamiento
            var availableSpaces = await _parkingSpaceRepository.GetAvailableSpacesAsync(createReservationDto.ParkingId);
            ParkingSpace? availableSpace = null;

            // Buscar el primer espacio sin conflictos de horario
            foreach (var space in availableSpaces)
            {
                var hasConflict = await _reservationRepository.HasConflictingReservationAsync(
                    space.Id, 
                    createReservationDto.StartTime, 
                    createReservationDto.EndTime);
                
                if (!hasConflict)
                {
                    availableSpace = space;
                    break;
                }
            }

            if (availableSpace == null)
            {
                throw new DomainException("No hay espacios disponibles sin conflictos de horario en este estacionamiento.");
            }

            var reservation = new Reservation
            {
                UserId = createReservationDto.UserId,
                ParkingId = createReservationDto.ParkingId,
                ParkingSpaceId = availableSpace.Id,
                StartTime = createReservationDto.StartTime,
                EndTime = createReservationDto.EndTime,
                TotalAmount = createReservationDto.TotalAmount,
                Status = ReservationStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                Notes = null
            };

            var createdReservation = await _reservationRepository.AddAsync(reservation);
            return MapToDto(createdReservation);
        }

        public async Task<ReservationDto> UpdateReservationAsync(int id, UpdateReservationDto updateReservationDto)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null)
            {
                throw new NotFoundException("Reserva", id);
            }

            // Validar que no hay conflictos de horario
            if (await _reservationRepository.HasConflictingReservationAsync(
                reservation.ParkingSpaceId, 
                updateReservationDto.StartTime, 
                updateReservationDto.EndTime))
            {
                throw new DomainException("Ya existe una reserva para este espacio en el horario especificado.");
            }

            reservation.StartTime = updateReservationDto.StartTime;
            reservation.EndTime = updateReservationDto.EndTime;
            reservation.Notes = updateReservationDto.Notes;

            await _reservationRepository.UpdateAsync(reservation);
            return MapToDto(reservation);
        }

        public async Task DeleteReservationAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null)
            {
                throw new NotFoundException("Reserva", id);
            }

            await _reservationRepository.DeleteAsync(id);
        }

        public async Task<bool> ReservationExistsAsync(int id)
        {
            return await _reservationRepository.ExistsAsync(id);
        }

        public async Task<bool> HasConflictingReservationAsync(int parkingSpaceId, DateTime startTime, DateTime endTime)
        {
            return await _reservationRepository.HasConflictingReservationAsync(parkingSpaceId, startTime, endTime);
        }

        public async Task<ReservationDto> ConfirmReservationAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null)
            {
                throw new NotFoundException("Reserva", id);
            }

            await _reservationRepository.UpdateStatusAsync(id, ReservationStatus.Confirmed);
            return MapToDto(reservation);
        }

        public async Task<ReservationDto> CancelReservationAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null)
            {
                throw new NotFoundException("Reserva", id);
            }

            await _reservationRepository.UpdateStatusAsync(id, ReservationStatus.Cancelled);
            return MapToDto(reservation);
        }

        public async Task<ReservationDto> CheckInReservationAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null)
            {
                throw new NotFoundException("Reserva", id);
            }

            reservation.CheckInTime = DateTime.UtcNow;
            reservation.Status = ReservationStatus.Active;

            await _reservationRepository.UpdateAsync(reservation);
            return MapToDto(reservation);
        }

        public async Task<ReservationDto> CheckOutReservationAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null)
            {
                throw new NotFoundException("Reserva", id);
            }

            reservation.CheckOutTime = DateTime.UtcNow;
            reservation.Status = ReservationStatus.Completed;

            await _reservationRepository.UpdateAsync(reservation);
            return MapToDto(reservation);
        }

        private static ReservationDto MapToDto(Reservation reservation)
        {
            return new ReservationDto
            {
                Id = reservation.Id,
                UserId = reservation.UserId,
                ParkingId = reservation.ParkingId,
                ParkingSpaceId = reservation.ParkingSpaceId,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                TotalAmount = reservation.TotalAmount,
                Status = reservation.Status.ToString(),
                PaymentStatus = reservation.PaymentStatus.ToString(),
                Notes = reservation.Notes,
                CheckInTime = reservation.CheckInTime,
                CheckOutTime = reservation.CheckOutTime,
                CreatedAt = reservation.CreatedAt,
                User = reservation.User != null ? new UserDto
                {
                    Id = reservation.User.Id,
                    FirstName = reservation.User.FirstName,
                    LastName = reservation.User.LastName,
                    Email = reservation.User.Email,
                    PhoneNumber = reservation.User.PhoneNumber,
                    LicensePlate = reservation.User.LicensePlate,
                    Role = reservation.User.Role.ToString(),
                    CreatedAt = reservation.User.CreatedAt,
                    IsActive = reservation.User.IsActive
                } : null,
                Parking = reservation.Parking != null ? new ParkingDto
                {
                    Id = reservation.Parking.Id,
                    Name = reservation.Parking.Name,
                    Address = reservation.Parking.Address,
                    City = reservation.Parking.City,
                    PostalCode = reservation.Parking.PostalCode,
                    Latitude = reservation.Parking.Latitude,
                    Longitude = reservation.Parking.Longitude,
                    TotalSpaces = reservation.Parking.TotalSpaces,
                    AvailableSpaces = reservation.Parking.AvailableSpaces,
                    HourlyRate = reservation.Parking.HourlyRate,
                    DailyRate = reservation.Parking.DailyRate,
                    Status = reservation.Parking.Status.ToString(),
                    Description = reservation.Parking.Description,
                    ImageUrl = reservation.Parking.ImageUrl,
                    CreatedAt = reservation.Parking.CreatedAt,
                    IsActive = reservation.Parking.IsActive
                } : null,
                ParkingSpace = reservation.ParkingSpace != null ? new ParkingSpaceDto
                {
                    Id = reservation.ParkingSpace.Id,
                    SpaceNumber = reservation.ParkingSpace.SpaceNumber,
                    ParkingId = reservation.ParkingSpace.ParkingId,
                    Type = reservation.ParkingSpace.Type.ToString(),
                    Status = reservation.ParkingSpace.Status.ToString(),
                    SpecialRate = reservation.ParkingSpace.SpecialRate,
                    Notes = reservation.ParkingSpace.Notes,
                    CreatedAt = reservation.ParkingSpace.CreatedAt,
                    IsActive = reservation.ParkingSpace.IsActive
                } : null
            };
        }
    }
} 