using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentalApp.Entities
{
    public class Rental
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid BookingNumber { get; set; }
        public string HashPersonalNr { get; set; }
        public Customer Customer { get; set; }
        public int RentalObjectId { get; set; }
        public Car RentalObject { get; set; }
        public DateTime TimeOfRent { get; set; }
    }
}
