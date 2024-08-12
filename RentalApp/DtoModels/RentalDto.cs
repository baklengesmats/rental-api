using RentalApp.Entities;

namespace RentalApp.DtoModels
{
    public class RentalDto
    {
        public Guid BookingNumber { get; set; }
        public string PersonNumber { get; set; }
        public Car RentalObject { get; set; }
        public DateTime TimeOfReleasing { get; set; }
    }
}
