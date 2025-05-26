using ContactApp.Api.Data;
using ContactApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactApp.Api.Controllers
{
    [ApiController] // Oznacza, �e to kontroler API
    [Route("api/[controller]")] // Definiuje bazow� �cie�k� dla tego kontrolera (np. /api/Contacts)
    public class ContactsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Wstrzykni�cie zale�no�ci (Dependency Injection) - kontekst bazy danych
        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        // Zwraca list� wszystkich kontakt�w
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            // Pobieramy kontakty, do��czaj�c powi�zane kategorie i podkategorie
            // U�ywamy Include, aby Entity Framework za�adowa� powi�zane dane
            // (bez tego Kategoria i Podkategoria by�yby null, je�li nie u�ywasz lazy loading)
            return await _context.Contacts
                                 .Include(c => c.Kategoria)
                                 .Include(c => c.Podkategoria)
                                 .ToListAsync();
        }

        // GET: api/Contacts/5
        // Zwraca szczeg�y konkretnego kontaktu na podstawie ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            // Znajdujemy kontakt po ID, r�wnie� do��czaj�c kategorie i podkategorie
            var contact = await _context.Contacts
                                        .Include(c => c.Kategoria)
                                        .Include(c => c.Podkategoria)
                                        .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null)
            {
                return NotFound(); // Zwraca 404 Not Found, je�li kontakt nie istnieje
            }

            return contact; // Zwraca znaleziony kontakt
        }

        // Na tym etapie nie implementujemy jeszcze POST, PUT, DELETE,
        // poniewa� wymagaj� one logowania. Zostan� dodane p�niej.
    }
}
