using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FooDOC.api.Data;
using Microsoft.EntityFrameworkCore;
using FooDOC.api.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FooDOC.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Konstruktor
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        
        // Hämta alla produkter
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products
                .Select(p => new Product { Id = p.Id, Name = p.Name, MinCookingTemp = p.MinCookingTemp })
                .ToListAsync();
            return Ok(products);
        }

        // Lägg till en ny produkt
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Name))
            {
                return BadRequest("Invalid product data.");
            }
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        
        // Hämta en specifik produkt
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // Uppdatera en befintlig produkt
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            if (id != updatedProduct.Id)
            {
                return BadRequest("Product ID mismatch.");
            }

            // Hämta produkten från databasen
            var productToUpdate = await _context.Products.FindAsync(id);

            if (productToUpdate == null)
            {
                return NotFound();
            }

            // Uppdatera egenskaperna med den nya datan från frontend
            productToUpdate.Name = updatedProduct.Name;
            productToUpdate.MinCookingTemp = updatedProduct.MinCookingTemp;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Hantera fall där produkten har tagits bort av en annan användare
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            // 204 No Content är en standardstatuskod för en lyckad PUT/PATCH-begäran som inte returnerar en resurs.
            return NoContent();
        }
        
        // Hjälpmetod för att kontrollera om en produkt existerar
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}