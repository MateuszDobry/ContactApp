using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactApp.Api.Models
{
    // Reprezentuje pojedynczy kontakt w bazie danych
    public class Contact
    {
        [Key] // Klucz g³ówny
        public int Id { get; set; }

        [Required(ErrorMessage = "Imiê jest wymagane.")] // Wymagane pole
        [MaxLength(100)] // Maksymalna d³ugoœæ
        public string Imie { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [MaxLength(100)]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Adres email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawid³owy format adresu email.")] // Walidacja formatu formatu email
        [MaxLength(255)]
        public string Email { get; set; }

        // Has³o bêdzie przechowywane w postaci zahashowanej
        [Required(ErrorMessage = "Has³o jest wymagane.")]
        public string HasloHash { get; set; } // Zmieniono nazwê na HasloHash dla jasnoœci

        [Required(ErrorMessage = "Kategoria jest wymagana.")]
        public int KategoriaId { get; set; } // Klucz obcy do tabeli Kategoria

        [ForeignKey("KategoriaId")] // Definicja klucza obcego
        public virtual ContactCategory Kategoria { get; set; } // Dodano s³owo kluczowe 'virtual'

        // Podkategoria mo¿e byæ opcjonalna lub zale¿na od kategorii
        public int? PodkategoriaId { get; set; } // Klucz obcy do tabeli Podkategoria (mo¿e byæ null)

        [ForeignKey("PodkategoriaId")]
        public virtual ContactSubcategory Podkategoria { get; set; } // Dodano s³owo kluczowe 'virtual'

        [Phone(ErrorMessage = "Nieprawid³owy format numeru telefonu.")] // Walidacja formatu telefonu
        [MaxLength(20)]
        public string Telefon { get; set; }

        [DataType(DataType.Date)] // Typ danych daty
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DataUrodzenia { get; set; } // Data urodzenia (mo¿e byæ null)
    }
}