using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ContactApp.Api.Models;

// Represents a subcategory of a contact, linked to a parent category
public class ContactSubcategory
{
    [Key] // Primary key
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nazwa { get; set; }

    // Optional foreign key to a parent category
    public int? KategoriaId { get; set; }

    [ForeignKey("KategoriaId")]
    public virtual ContactCategory Kategoria { get; set; }
}
