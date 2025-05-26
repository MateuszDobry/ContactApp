using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactApp.Api.Models
{
    // Reprezentuje pojedynczy kontakt w bazie danych
    public class Contact
    {
        [Key] // Klucz g��wny
        public int Id { get; set; }

        [Required(ErrorMessage = "Imi� jest wymagane.")] // Wymagane pole
        [MaxLength(100)] // Maksymalna d�ugo��
        public string Imie { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [MaxLength(100)]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Adres email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawid�owy format adresu email.")] // Walidacja formatu formatu email
        [MaxLength(255)]
        public string Email { get; set; }

        // Has�o b�dzie przechowywane w postaci zahashowanej
        [Required(ErrorMessage = "Has�o jest wymagane.")]
        public string HasloHash { get; set; } // Zmieniono nazw� na HasloHash dla jasno�ci

        [Required(ErrorMessage = "Kategoria jest wymagana.")]
        public int KategoriaId { get; set; } // Klucz obcy do tabeli Kategoria

        [ForeignKey("KategoriaId")] // Definicja klucza obcego
        public virtual ContactCategory Kategoria { get; set; } // Dodano s�owo kluczowe 'virtual'

        // Podkategoria mo�e by� opcjonalna lub zale�na od kategorii
        public int? PodkategoriaId { get; set; } // Klucz obcy do tabeli Podkategoria (mo�e by� null)

        [ForeignKey("PodkategoriaId")]
        public virtual ContactSubcategory Podkategoria { get; set; } // Dodano s�owo kluczowe 'virtual'

        [Phone(ErrorMessage = "Nieprawid�owy format numeru telefonu.")] // Walidacja formatu telefonu
        [MaxLength(20)]
        public string Telefon { get; set; }

        [DataType(DataType.Date)] // Typ danych daty
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DataUrodzenia { get; set; } // Data urodzenia (mo�e by� null)
    }
}