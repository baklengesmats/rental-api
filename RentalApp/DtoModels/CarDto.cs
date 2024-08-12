using RentalApp.Entities;

namespace RentalApp.DtoModels
{
    public class CarResponseDto
    {
        public string RegistrationId { get; set; }
        public int Km { get; set; }
        public double BaseDayPrice { get; set; }
        public double? BaseKmPrice { get; set; }
        public CarType Type { get; set; }
    }
}
