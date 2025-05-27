using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ContactApp.Api.Data;

namespace ContactApp.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Imie { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string HasloHash { get; set; } = string.Empty;

        // Brakuj�ca w�a�ciwo�� - dodaj j�
        public string Haslo => HasloHash;  // Je�li chcesz u�ywa� Haslo jako alias
    }
}