using RentalApp.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentalApp.DtoModels
{
    public class ReturnDto
    {
            public int Id { get; set; }
            public string BookingNumber { get; set; }
            public DateTime TimeOfRent {  get; set; }
            public DateTime TimeOfReturn { get; set; }
            public string RegistrationId { get; set; }
            public double Price { get; set; }
    }
}
