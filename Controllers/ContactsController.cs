using Microsoft.AspNetCore.Mvc;
using ContactApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ContactApp.Api.Data;

namespace ContactApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requires the user to be authenticated for all actions unless explicitly overridden
    public class ContactsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/contacts
        [HttpGet]
        [AllowAnonymous] // Overrides [Authorize] – allows anyone to view the contact list
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            // Eager-load related category and subcategory to avoid lazy loading issues
            return await _context.Contacts
                                 .Include(c => c.Kategoria)
                                 .Include(c => c.Podkategoria)
                                 .ToListAsync();
        }

        // GET: api/contacts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            // Fetch the contact with its related category and subcategory
            var contact = await _context.Contacts
                                        .Include(c => c.Kategoria)
                                        .Include(c => c.Podkategoria)
                                        .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null)
            {
                return NotFound(); // 404 if contact with given ID does not exist
            }

            return contact;
        }

        // POST: api/contacts
        [HttpPost]
        public async Task<ActionResult<Contact>> CreateContact(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            // Returns 201 Created with location header set to newly created resource
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }
    }
}
