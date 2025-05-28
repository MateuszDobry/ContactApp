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

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        // Check if a user with the same email already exists
        if (_context.Users.Any(u => u.Email == model.Email))
            return BadRequest("Użytkownik z tym e-mailem już istnieje");

        var user = new User
        {
            Imie = model.Imie,
            Email = model.Email,
            HasloHash = HashPassword(model.Haslo) // Store hashed password instead of plain text
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Rejestracja udana!" });
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        // Find user by email
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);

        // Validate password by comparing hashes
        if (user == null || user.HasloHash != HashPassword(login.Haslo))
            return Unauthorized("Nieprawidłowy email lub hasło");

        // JWT token generation for authentication
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("your-secret-key-here-that-is-long-enough-at-least-32-chars"); // Must match server configuration
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                // Claims identify the user; SID and Name both set to user ID here
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7), // Token valid for 7 days
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),

            Issuer = "ContactApp",     // Identifies the token issuer
            Audience = "ContactApp",   // Identifies the intended recipient
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new
        {
            userId = user.Id,
            userName = user.Imie,
            token = tokenHandler.WriteToken(token) // Return the serialized JWT token
        });
    }

    // Helper method to hash a password using SHA256
    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
                builder.Append(b.ToString("x2")); // Convert byte to hex string
            return builder.ToString();
        }
    }
}

// DTO for login input validation
public class LoginModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Haslo { get; set; }
}
