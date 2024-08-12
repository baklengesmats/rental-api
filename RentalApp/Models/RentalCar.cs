using Microsoft.AspNetCore.DataProtection.KeyManagement;
using RentalApp.Entities;
using RentalApp.Models.Price;

namespace RentalApp.Models
{
    public class RentalCar 
    {
        public string RegistrationNumber { get; set; }
        public double BaseDayPrice { get; set; }
        public int Days { get; set; }
        public int CurrentKm { get; set; }

        private IPrice _priceCal; 
        public double Price()
        {
            return _priceCal.CalculatePrice(Days, BaseDayPrice);
        }
        public RentalCar(string registrationNumber, double baseDayPrice, int days,  int? km, IPrice priceCal)
        {
            RegistrationNumber = registrationNumber;
            BaseDayPrice = baseDayPrice;
            Days = days;
            CurrentKm = km ?? 0;
            _priceCal = priceCal;
        }
    }
}
