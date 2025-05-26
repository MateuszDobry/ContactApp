using System.ComponentModel.DataAnnotations;

namespace ContactApp.Api.Models
{
    // Reprezentuje s³ownik kategorii kontaktów (s³u¿bowy, prywatny, inny)
    public class ContactCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nazwa { get; set; } 
    }
}