using RentalApp.Entities;

namespace RentalApp.DtoModels
{
    public class RentalResponseDto
    {
        public Guid BookingNumber { get; set; }
        public string PersonNumber { get; set; }
        public CarResponseDto RentalObject { get; set; }
        public DateTime TimeOfRent { get; set; }
    }
}
