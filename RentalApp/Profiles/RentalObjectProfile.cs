using AutoMapper;
using RentalApp.DtoModels;
using RentalApp.Entities;

namespace RentalApp.Profiles
{
    public class RentalObjectProfile:Profile
    {
        public RentalObjectProfile() 
        {
            CreateMap<Car, CarDto>();
            CreateMap<Car, CarResponseDto>();
            CreateMap<CarResponseDto, CarDto>();
            CreateMap<CarDto, Car>();
        }
    }
}
