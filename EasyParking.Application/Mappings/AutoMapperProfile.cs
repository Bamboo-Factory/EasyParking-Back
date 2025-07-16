using AutoMapper;
using EasyParking.Application.DTOs;
using EasyParking.Core.Entities;

namespace EasyParking.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Mapeos de User
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
            
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse<UserRole>(src.Role)))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            // Mapeos de Parking
            CreateMap<Parking, ParkingDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            
            CreateMap<ParkingDto, Parking>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ParkingStatus>(src.Status)))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            // Mapeos de ParkingSpace
            CreateMap<ParkingSpace, ParkingSpaceDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            
            CreateMap<ParkingSpaceDto, ParkingSpace>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<SpaceType>(src.Type)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<SpaceStatus>(src.Status)))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            // Mapeos de Reservation
            CreateMap<Reservation, ReservationDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()));
            
            CreateMap<ReservationDto, Reservation>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ReservationStatus>(src.Status)))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => Enum.Parse<PaymentStatus>(src.PaymentStatus)))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());
        }
    }
} 