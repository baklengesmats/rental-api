using AutoMapper;
using RentalApp.DtoModels;
using RentalApp.Entities;

namespace RentalApp.Profiles
{
    public class RentalProfile : Profile
    {
        public RentalProfile() {
            CreateMap<RegisterRentalDto, Entities.Rental>();
            CreateMap<Entities.Rental, RentalDto>()
                .ForMember(dest => dest.PersonNumber, opt => opt.MapFrom(src => src.Customer.PersonNumber));
            CreateMap<Rental, RentalResponseDto>()
                .ForMember(dest => dest.PersonNumber, opt => opt.MapFrom(src => src.Customer.PersonNumber));
        }
    }
}
