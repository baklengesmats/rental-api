using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RentalApp.Models.Price;

namespace RentalApp.Entities
{
    public class Returning
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string BookingNumber { get; set; }
        public DateTime TimeOfReturn {  get; set; }
        public DateTime TimeOfRent {  get; set; }
        public string RegistrationId { get; set; }
        public double Price { get; set; }
    }
}
