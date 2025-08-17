using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlphaProject.API.Models;
using AlphaProject.Shared.Dtos;
using AutoMapper;

namespace AlphaProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly  IMapper _mapper;

        public OrderItemsController(AppDbContext context, IMapper mappingProfile)
        {
            _context = context;
            _mapper = mappingProfile;
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderItems()
        {
            var orderItems = await _context.OrderItems.ToListAsync();
            return Ok(_mapper.Map<List<OrderItemDto>>(orderItems));
        }

        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemDto>> GetOrderItem(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<OrderItemDto>(orderItem));
        }

        // PUT: api/OrderItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(int id, OrderItemDto orderItemDto)
        {
            if (id != orderItemDto.OrderItemId)
            {
                return BadRequest();
            }
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            _mapper.Map(orderItemDto, orderItem);
            _context.Entry(orderItem).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(id))
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

        // POST: api/OrderItems
        [HttpPost]
        public async Task<ActionResult<OrderItemDto>> PostOrderItem(OrderItemDto orderItemDto)
        {
            var orderItem = _mapper.Map<OrderItem>(orderItemDto);
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
            var createdDto = _mapper.Map<OrderItemDto>(orderItem);
            return CreatedAtAction("GetOrderItem", new { id = orderItem.OrderItemId }, createdDto);
        }

        // DELETE: api/OrderItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.OrderItemId == id);
        }
    }
}
