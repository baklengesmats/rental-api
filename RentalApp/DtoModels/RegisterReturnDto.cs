using RentalApp.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentalApp.DtoModels
{
    public class RegisterReturnDto
    {
        public string BookingNumber { get; set; }
        public DateTime TimeOfReturn { get; set; }
        public int EndedKm { get; set; }
    }
}
