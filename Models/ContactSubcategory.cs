using System.ComponentModel.DataAnnotations;
using ContactApp.Api.Models;
using System.ComponentModel.DataAnnotations.Schema;


public class ContactSubcategory
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nazwa { get; set; }

    public int? KategoriaId { get; set; }

    [ForeignKey("KategoriaId")]
    public virtual ContactCategory Kategoria { get; set; }
}
