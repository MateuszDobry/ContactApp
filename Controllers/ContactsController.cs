using ContactApp.Api.Data;
using ContactApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ContactApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
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
        [Authorize]
        public async Task<ActionResult<Contact>> CreateContact(Contact contact)
        {
            // 1. Add the new contact to the DbContext
            _context.Contacts.Add(contact);

            // 2. Save changes to the database
            await _context.SaveChangesAsync();

            // 3. Return a 201 CreatedAtAction response
            // This is the common practice for HTTP POST creating a resource.
            // It tells the client where the newly created resource can be found.
            // "GetContact" is the action name that retrieves a single contact.
            // new { id = contact.Id } provides the route values for GetContact.
            // 'contact' is the created object to be returned in the response body.
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }

        // Na tym etapie nie implementujemy jeszcze POST, PUT, DELETE,
        // poniewa¿ wymagaj¹ one logowania. Zostan¹ dodane póŸniej.
    }
}