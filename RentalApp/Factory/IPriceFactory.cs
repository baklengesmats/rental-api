using RentalApp.DtoModels;
using RentalApp.Entities;
using RentalApp.Models.Price;

namespace RentalApp.Factory
{
    public interface IPriceFactory
    {
        IPrice CreatePriceStrategy(CarDto car, int usedKm);
    }
}
