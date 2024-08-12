using RentalApp.DtoModels;
using RentalApp.Entities;

namespace RentalApp.Services
{
    public interface IRentalObjectService
    {
        Task<CarDto> GetRentalObjectById(string registraionId);
        Task<IEnumerable<CarDto>> GetAllRentalObjects(CarType? type);
        Task UpdateRentalObject(CarDto rentalObject);
    }
}
