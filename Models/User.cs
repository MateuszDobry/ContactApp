using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using ContactApp.Api.Data; 

namespace ContactApp.Api.Models
{
    public class User
    {
   
        // Gets or sets the unique identifier for the user.
        public int Id { get; set; }

   
        [Required] // Ensures the 'Imie' (First Name) field is not null or empty.
        [MaxLength(100)] 
        public string Imie { get; set; } = string.Empty; 


        [Required]
        [EmailAddress] // Validates the email format.
        public string Email { get; set; } = string.Empty; 

        [Required] 
        public string HasloHash { get; set; } = string.Empty; 

        // This property is a computed property, meaning it doesn't have a backing field in the database.
        // It simply returns the value of 'HasloHash'. This might be an artifact of renaming
        // from 'Haslo' to 'HasloHash' for clarity on password security,
        // while still allowing 'Haslo' to be referenced in existing code.
        public string Haslo => HasloHash;
    }
}