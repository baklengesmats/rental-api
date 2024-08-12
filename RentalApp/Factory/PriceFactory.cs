using Microsoft.AspNetCore.DataProtection.KeyManagement;
using RentalApp.DtoModels;
using RentalApp.Entities;
using RentalApp.Models.Price;

namespace RentalApp.Factory
{
    public class PriceFactory : IPriceFactory
    {
        public IPrice CreatePriceStrategy(CarDto car, int usedKm)
        {
            return car.Type switch
            {
                CarType.SmallCar => new SmallCarPrice(),
                CarType.CombiCar => new CombiCarPrice(usedKm, car.BaseKmPrice ?? 0),
                CarType.Truck => new TruckPrice(usedKm, car.BaseKmPrice ?? 0),
                _ => throw new ArgumentOutOfRangeException(nameof(car.Type), $"No pricing strategy defined for {car.Type}")
            };
        }
    }
}
