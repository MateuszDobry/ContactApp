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

        // Brakuj¹ca w³aœciwoœæ - dodaj j¹
        public string Haslo => HasloHash;  // Jeœli chcesz u¿ywaæ Haslo jako alias
    }
}