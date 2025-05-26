using ContactApp.Api.Models;
using ContactApp.Api.Data;    
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ContactApp.Api.Controllers
{
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
        public async Task<IActionResult> Register(User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
                return BadRequest("U¿ytkownik z tym e-mailem ju¿ istnieje");

            // W rzeczywistej aplikacji u¿yj hashowania np. BCrypt
            user.HasloHash = HashPassword(user.HasloHash);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Rejestracja udana!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);

            if (user == null || user.HasloHash != HashPassword(login.Haslo))
                return Unauthorized("Nieprawid³owy email lub has³o");

            // W rzeczywistoœci tu generujesz JWT token
            return Ok(new { userId = user.Id, userName = user.Imie });
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
        public string Email { get; set; }
        public string Haslo { get; set; }
    }
}