using ContactApp.Api.Data;
using ContactApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactApp.Api.Controllers
{
    [ApiController] // Oznacza, ¿e to kontroler API
    [Route("api/[controller]")] // Definiuje bazow¹ œcie¿kê dla tego kontrolera (np. /api/Contacts)
    public class ContactsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Wstrzykniêcie zale¿noœci (Dependency Injection) - kontekst bazy danych
        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        // Zwraca listê wszystkich kontaktów
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            // Pobieramy kontakty, do³¹czaj¹c powi¹zane kategorie i podkategorie
            // U¿ywamy Include, aby Entity Framework za³adowa³ powi¹zane dane
            // (bez tego Kategoria i Podkategoria by³yby null, jeœli nie u¿ywasz lazy loading)
            return await _context.Contacts
                                 .Include(c => c.Kategoria)
                                 .Include(c => c.Podkategoria)
                                 .ToListAsync();
        }

        // GET: api/Contacts/5
        // Zwraca szczegó³y konkretnego kontaktu na podstawie ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            // Znajdujemy kontakt po ID, równie¿ do³¹czaj¹c kategorie i podkategorie
            var contact = await _context.Contacts
                                        .Include(c => c.Kategoria)
                                        .Include(c => c.Podkategoria)
                                        .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null)
            {
                return NotFound(); // Zwraca 404 Not Found, jeœli kontakt nie istnieje
            }

            return contact; // Zwraca znaleziony kontakt
        }

        // Na tym etapie nie implementujemy jeszcze POST, PUT, DELETE,
        // poniewa¿ wymagaj¹ one logowania. Zostan¹ dodane póŸniej.
    }
}
