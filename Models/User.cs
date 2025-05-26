
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // <--- ADD THIS LINE

namespace ContactApp.Api.Models // <--- ADD THIS NAMESPACE DECLARATION (based on your project structure)
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Imie { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string HasloHash { get; set; }
    } // <--- REMOVE THE EXTRA CURLY BRACE HERE IF IT WAS `}}`
}