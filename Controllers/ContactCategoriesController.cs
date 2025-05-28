using Microsoft.AspNetCore.Mvc;
using ContactApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using ContactApp.Api.Data;

namespace ContactApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContactCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/contactcategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactCategory>>> GetContactCategories()
        {
            // Returns all contact categories from the database as a list
            return await _context.ContactCategories.ToListAsync();
        }
    }
}
