using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ContactApp.Api.Data;
using ContactApp.Api.Models;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;




[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (_context.Users.Any(u => u.Email == model.Email))
            return BadRequest("Użytkownik z tym e-mailem już istnieje");

        var user = new User
        {
            Imie = model.Imie,
            Email = model.Email,
            HasloHash = HashPassword(model.Haslo) // ✅ Używasz hasła z modelu
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Rejestracja udana!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);

        if (user == null || user.HasloHash != HashPassword(login.Haslo))
            return Unauthorized("Nieprawidłowy email lub hasło");
       
            // Generuj token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("your-secret-key-here-that-is-long-enough-at-least-32-chars");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),

            Issuer = "ContactApp",     
            Audience = "ContactApp",
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new
        {
            userId = user.Id,
            userName = user.Imie,
            token = tokenHandler.WriteToken(token)
        });
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }
}

public class LoginModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Haslo { get; set; }
}