using EasyParking.Application.DTOs;
using EasyParking.Application.Interfaces;
using EasyParking.Core.Interfaces;
using EasyParking.Core.Entities;
using EasyParking.Core.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace EasyParking.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            if (await _userRepository.EmailExistsAsync(createUserDto.Email))
            {
                throw new DomainException($"El email '{createUserDto.Email}' ya está registrado.");
            }

            if (await _userRepository.LicensePlateExistsAsync(createUserDto.LicensePlate))
            {
                throw new DomainException($"La placa '{createUserDto.LicensePlate}' ya está registrada.");
            }

            var user = new User
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                PhoneNumber = createUserDto.PhoneNumber,
                LicensePlate = createUserDto.LicensePlate,
                PasswordHash = HashPassword(createUserDto.Password),
                Role = Enum.Parse<UserRole>(createUserDto.Role)
            };

            var createdUser = await _userRepository.AddAsync(user);
            return MapToDto(createdUser);
        }

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("Usuario", id);
            }

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.PhoneNumber = updateUserDto.PhoneNumber;
            user.LicensePlate = updateUserDto.LicensePlate;

            await _userRepository.UpdateAsync(user);
            return MapToDto(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("Usuario", id);
            }

            await _userRepository.DeleteAsync(id);
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await _userRepository.ExistsAsync(id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userRepository.EmailExistsAsync(email);
        }

        public async Task<bool> LicensePlateExistsAsync(string licensePlate)
        {
            return await _userRepository.LicensePlateExistsAsync(licensePlate);
        }

        public async Task<AuthenticatedUserResponseDto?> ValidateUserCredentialsAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
                return null;

            var inputHash = ComputeSha256Hash(password);
            if (user.PasswordHash == inputHash || user.PasswordHash == password)
            {
                return new AuthenticatedUserResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = user.Role.ToString()
                };
            }
            return null;
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                LicensePlate = user.LicensePlate,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(rawData);
                var hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
            }
        }
    }
} 