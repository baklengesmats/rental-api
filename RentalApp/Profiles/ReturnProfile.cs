using AutoMapper;
using RentalApp.DtoModels;
using RentalApp.Entities;

namespace RentalApp.Profiles
{
    public class ReturnProfile:Profile
    {
        public ReturnProfile() {
            CreateMap<Returning, ReturnDto>();
        }
    }
}
