using FooDOC.api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FooDOC.api.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace FooDOC.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CCPController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Konstruktor
        public CCPController(AppDbContext context)
        {
            _context = context;
        }

        // Skapa en ny kontrollpunkt
        [HttpPost]
        public async Task<IActionResult> CreateCCP([FromBody] TempCCP ccp)
        {
            if (ccp == null)
            {
                return BadRequest("Invalid data.");
            }

            // Använd den inkommande modellen direkt och sätt CreatedAt-tiden
            var newCCP = new TempCCP
            {
                Product = ccp.Product,
                Temp = ccp.Temp,
                Type = ccp.Type, // NYTT: Nu sparar vi även Type-egenskapen
                CreatedAt = DateTime.UtcNow
            };

            _context.TempCCPs.Add(newCCP);
            await _context.SaveChangesAsync();

            // Returnera den nyskapade resursen
            return CreatedAtAction(nameof(GetCCP), new { id = newCCP.Id }, newCCP);
        }

        // Hämta alla kontrollpunkter med filtreringsmöjligheter
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<TempCCP>>> GetCCPs(
            // Använd [FromQuery] för att hämta filterparametrar från URL-query-strängen
            [FromQuery] string? product,
            [FromQuery] string? type,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            // Börja med en IQueryable för att bygga upp frågan dynamiskt
            var query = _context.TempCCPs.AsQueryable();

            // 1. Filtrera på produkt om parametern är angiven
            if (!string.IsNullOrEmpty(product))
            {
                query = query.Where(c => c.Product == product);
            }

            // 2. Filtrera på typ om parametern är angiven
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(c => c.Type == type);
            }

            // 3. Filtrera på datumintervall om parametrarna är angivna
            if (startDate.HasValue)
            {
                // Jämför bara datumdelen för att inkludera hela dagen
                query = query.Where(c => c.CreatedAt.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                // Jämför bara datumdelen för att inkludera hela dagen
                query = query.Where(c => c.CreatedAt.Date <= endDate.Value.Date);
            }

            // Utför frågan mot databasen och välj ut de egenskaper du behöver
            var filteredCCPs = await query
                .Select(c => new TempCCP { Id = c.Id, Product = c.Product, Temp = c.Temp, CreatedAt = c.CreatedAt, Type = c.Type }) // NYTT: Se till att Type inkluderas i resultatet
                .ToListAsync();

            if (!filteredCCPs.Any())
            {
                // Returnera en tom lista istället för NotFound om inga resultat matchar
                return Ok(new List<TempCCP>());
            }

            return Ok(filteredCCPs);
        }

        // Hämta en specifik kontrollpunkt
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetCCP(int id)
        {
            var ccp = await _context.TempCCPs.FindAsync(id);

            if (ccp == null)
            {
                return NotFound();
            }

            // Returnera det specifika objektet med alla dess egenskaper
            return Ok(new TempCCP { Id = ccp.Id, Product = ccp.Product, Temp = ccp.Temp, CreatedAt = ccp.CreatedAt, Type = ccp.Type }); // NYTT: Se till att Type inkluderas här också
        }
    }
}