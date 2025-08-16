using AlphaProject.API.Models;
using AlphaProject.Shared.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public ProductsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }




        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _context.Products
                .Include(p=>p.OrderItems) // include orderitems to fill the navigation property
                .ToListAsync();

            return Ok(_mapper.Map<List<ProductDto>>(products));
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            // Carica anche gli OrderItems associati al prodotto
            var product = await _context.Products
                .Include(p => p.OrderItems)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            // Mappa il prodotto (con OrderItems) a ProductDto
            ProductDto pDto = _mapper.Map<ProductDto>(product);

            return pDto;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductDto productDto)
        {
            if (id != productDto.ProductId)
            {
                return BadRequest();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Mappa i campi da ProductDto a Product
            _mapper.Map(productDto, product);

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct(ProductDto productDto)
        {
            // Mappa il DTO in un'entità Product
            var product = _mapper.Map<Product>(productDto);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Mappa l'entità salvata in un DTO per la risposta
            var createdProductDto = _mapper.Map<ProductDto>(product);

            // Restituisce HTTP 201 (Created) con header "Location" che punta all'URL della nuova risorsa.
            // ASP.NET Core genera automaticamente l'URL cercando l'action "GetProduct" in questo controller,
            // e applicando i route template definiti dall'attributo [HttpGet("{id}")]. 
            // Il valore passato (new { id = product.ProductId }) viene usato per sostituire il parametro {id} nella route.
            // In questo modo il client ottiene sia i dati creati (nel body) sia il percorso per recuperarli via GET.
            return CreatedAtAction("GetProduct", new { id = product.ProductId }, createdProductDto);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
