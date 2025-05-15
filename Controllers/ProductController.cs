using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FooDOC.api.Data;
using Microsoft.EntityFrameworkCore;
using FooDOC.api.Models;

namespace FooDOC.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        //Konstruktor
        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Name))
            {
                return BadRequest("Ogiltigt produktnamn.");
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);

            
        }

    }
}
