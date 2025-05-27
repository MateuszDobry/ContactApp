using System.ComponentModel.DataAnnotations;

namespace ContactApp.Api.Models
{
	public class LoginModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string Haslo { get; set; } = string.Empty;
	}
}