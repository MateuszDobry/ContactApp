using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactApp.Api.Models
{
    // Represents an individual contact record in the database
    public class Contact
    {
        public Contact()
        {
            Imie = string.Empty;
            Nazwisko = string.Empty;
            Email = string.Empty;
            HasloHash = string.Empty;
            Telefon = string.Empty;
            DataUrodzenia = null;
        }

        [Key] // Primary key
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(100)]
        public string Imie { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(100)]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(255)]
        public string Email { get; set; }

        // Stores the password as a hashed value for security
        [Required(ErrorMessage = "Password is required.")]
        public string HasloHash { get; set; }

        // Foreign key to ContactCategory
        [Required(ErrorMessage = "Category is required.")]
        public int KategoriaId { get; set; }

        [ForeignKey("KategoriaId")]
        public virtual ContactCategory Kategoria { get; set; }

        // Optional foreign key to subcategory
        public int? PodkategoriaId { get; set; }

        [ForeignKey("PodkategoriaId")]
        public virtual ContactSubcategory Podkategoria { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [MaxLength(20)]
        public string Telefon { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DataUrodzenia { get; set; }
    }
}
