using RentalApp.DtoModels;
using RentalApp.Entities;

namespace RentalApp.Repositories
{
    public interface IRentalObjectRepository
    {
        Task<IEnumerable<Car>> GetRentalObjects(CarType? type, bool? isAvailable);
        Task<Car> GetRentalObject(string registraionId);
        Task UpdateRentalObject (CarDto rentalObject);
    }
}
