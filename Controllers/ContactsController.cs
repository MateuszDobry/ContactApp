using Microsoft.AspNetCore.Mvc;
using ContactApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ContactApp.Api.Data;



namespace ContactApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Wymaga logowania
    public class ContactsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous] // Lista kontaktów dostêpna dla wszystkich
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            return await _context.Contacts
                                 .Include(c => c.Kategoria)
                                 .Include(c => c.Podkategoria)
                                 .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _context.Contacts
                                       .Include(c => c.Kategoria)
                                       .Include(c => c.Podkategoria)
                                       .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        [HttpPost]
        public async Task<ActionResult<Contact>> CreateContact(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }
    }
}