using FooDOC.api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FooDOC.api.Models;
using Microsoft.AspNetCore.Authorization;

namespace FooDOC.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CCPController : ControllerBase
    {
        private readonly AppDbContext _context;

        //Konstruktor
        public CCPController (AppDbContext context)
        {
            _context = context;
        }

         // Skapa en ny kontrollpunkt (tempraturdatning)
        [HttpPost]
        public async Task<IActionResult> CreateCCP([FromBody] TempCCP ccp)
        {
            if (ccp == null)
            {
                return BadRequest("Invalid data.");
            }

            var newCCP = new TempCCP
            {
                Product = ccp.Product,
                Temp = ccp.Temp,
                CreatedAt = DateTime.UtcNow
            };

            _context.TempCCPs.Add(newCCP);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCCP), new { id = newCCP.Id }, newCCP);
        }

        // Hämta alla kontrollpunkter
        [HttpGet]
        [Authorize (Roles = "admin")]
        public async Task<IActionResult> GetCCPs()
        {
            var CCPs = await _context.TempCCPs
                .Select(c => new TempCCP { Id = c.Id, Product = c.Product, Temp = c.Temp, CreatedAt = c.CreatedAt }).ToListAsync();

            return Ok(CCPs);
        }

        // Hämta en kontrollpunkt
        [HttpGet("{id}")]
        [Authorize (Roles = "admin")]
        public async Task<IActionResult> GetCCP(int id)
        {
            var ccp = await _context.TempCCPs.FindAsync(id);

            if (ccp == null)
            {
                return NotFound();
            }

            return Ok(new TempCCP { Id = ccp.Id, Product = ccp.Product, Temp = ccp.Temp, CreatedAt = ccp.CreatedAt });

        }
    }
}
