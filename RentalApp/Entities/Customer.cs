using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalApp.Entities
{
    public class Customer
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string PersonNumber { get; set; }

        [Required]
        [Key]
        public string HashedPersonNumber { get; set; }
    }
}
