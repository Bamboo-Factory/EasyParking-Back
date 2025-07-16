using AutoMapper;
using EasyParking.Application.DTOs;
using EasyParking.Application.Interfaces;
using EasyParking.Core.Entities;
using EasyParking.Core.Interfaces;
using EasyParking.Core.Exceptions;

namespace EasyParking.Application.Services
{
    public class ParkingService : IParkingService
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ParkingService(IParkingRepository parkingRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _parkingRepository = parkingRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParkingDto>> GetAllAsync()
        {
            var parkings = await _parkingRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ParkingDto>>(parkings);
        }

        public async Task<ParkingDto?> GetByIdAsync(int id)
        {
            var parking = await _parkingRepository.GetByIdAsync(id);
            return _mapper.Map<ParkingDto>(parking);
        }

        public async Task<IEnumerable<ParkingDto>> GetByCityAsync(string city)
        {
            var parkings = await _parkingRepository.GetByCityAsync(city);
            return _mapper.Map<IEnumerable<ParkingDto>>(parkings);
        }

        public async Task<IEnumerable<ParkingDto>> GetAvailableParkingsAsync()
        {
            var parkings = await _parkingRepository.GetAvailableParkingsAsync();
            return _mapper.Map<IEnumerable<ParkingDto>>(parkings);
        }

        public async Task<IEnumerable<ParkingDto>> SearchByLocationAsync(decimal latitude, decimal longitude, double radiusKm)
        {
            var parkings = await _parkingRepository.SearchByLocationAsync(latitude, longitude, radiusKm);
            return _mapper.Map<IEnumerable<ParkingDto>>(parkings);
        }

        public async Task<IEnumerable<ParkingDto>> SearchByLocationOptimizedAsync(decimal latitude, decimal longitude, double radiusKm)
        {
            var parkings = await _parkingRepository.SearchByLocationOptimizedAsync(latitude, longitude, radiusKm);
            return _mapper.Map<IEnumerable<ParkingDto>>(parkings);
        }

        public async Task<ParkingDto> CreateAsync(ParkingDto parkingDto)
        {
            var parking = _mapper.Map<Parking>(parkingDto);
            parking.CreatedAt = DateTime.UtcNow;
            parking.IsActive = true;

            await _parkingRepository.AddAsync(parking);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ParkingDto>(parking);
        }

        public async Task<ParkingDto> UpdateAsync(int id, ParkingDto parkingDto)
        {
            var existingParking = await _parkingRepository.GetByIdAsync(id);
            if (existingParking == null)
                throw new NotFoundException($"Estacionamiento con ID {id} no encontrado");

            _mapper.Map(parkingDto, existingParking);
            existingParking.UpdatedAt = DateTime.UtcNow;

            await _parkingRepository.UpdateAsync(existingParking);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ParkingDto>(existingParking);
        }

        public async Task DeleteAsync(int id)
        {
            var parking = await _parkingRepository.GetByIdAsync(id);
            if (parking == null)
                throw new NotFoundException($"Estacionamiento con ID {id} no encontrado");

            parking.IsActive = false;
            parking.UpdatedAt = DateTime.UtcNow;

            await _parkingRepository.UpdateAsync(parking);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ParkingExistsAsync(int id)
        {
            return await _parkingRepository.GetByIdAsync(id) != null;
        }

        public async Task UpdateAvailableSpacesAsync(int parkingId, int availableSpaces)
        {
            await _parkingRepository.UpdateAvailableSpacesAsync(parkingId, availableSpaces);
        }
    }
} 