using System.ComponentModel.DataAnnotations;
public class ContactSubcategory
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nazwa { get; set; } // Np. "Szef", "Klient", "Wspó³pracownik"

    // Opcjonalnie: Mo¿na dodaæ klucz obcy do ContactCategory,
    // aby powi¹zaæ podkategorie z konkretnymi kategoriami
    /*
     public int? KategoriaId { get; set; }
     [ForeignKey("KategoriaId")]
     public ContactCategory Kategoria { get; set; }
    */
}
