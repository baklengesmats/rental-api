using RentalApp.Entities;

namespace RentalApp.DtoModels
{
    public class CarDto
    {
        public string RegistrationId { get; set; }
        public int Km { get; set; }
        public double BaseDayPrice { get; set; }
        public double? BaseKmPrice { get; set; }
        public bool IsAvailable { get; set; }
        public CarType Type { get; set; }
    }
}
