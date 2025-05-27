using System.ComponentModel.DataAnnotations;

public class RegisterModel
{
	[Required]
	[MaxLength(100)]
	public string Imie { get; set; } = string.Empty;

	[Required]
	[EmailAddress]
	public string Email { get; set; } = string.Empty;

	[Required]
	public string Haslo { get; set; } = string.Empty;
}