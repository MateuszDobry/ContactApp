using System.ComponentModel.DataAnnotations;

namespace ContactApp.Api.Models
{
    // Reprezentuje s�ownik kategorii kontakt�w (s�u�bowy, prywatny, inny)
    public class ContactCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nazwa { get; set; } 
    }
}