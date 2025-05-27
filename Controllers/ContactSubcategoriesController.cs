using Microsoft.AspNetCore.Mvc;
using ContactApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using ContactApp.Api.Data;
using Microsoft.AspNetCore.Authorization;

namespace ContactApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactSubcategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContactSubcategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactSubcategory>>> GetContactSubcategories()
        {
            return await _context.ContactSubcategories.ToListAsync();
        }
    }
}