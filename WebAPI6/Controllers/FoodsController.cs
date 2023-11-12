using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI6.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly FoodStoreContext _context;

        public FoodsController(FoodStoreContext context)
        {
            _context = context;
        }

        // GET: api/Foods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoods()
        {
          if (_context.Foods == null)
          {
              return NotFound();
          }
            return await _context.Foods.ToListAsync();
        }

        // GET: api/Foods/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> GetFood(int id)
        {
          if (_context.Foods == null)
          {
              return NotFound();
          }
            var food = await _context.Foods.FindAsync(id);

            if (food == null)
            {
                return NotFound();
            }

            return food;
        }

        // PUT: api/Foods/Update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Update/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateFood(int id, Food updatedFood)
        {
            if (id != updatedFood.Id)
            {
                return BadRequest();
            }

            var existingFood = await _context.Foods!.FindAsync(id);

            if (existingFood == null)
            {
                return NotFound();
            }

            existingFood.Name = updatedFood.Name;
            existingFood.Description = updatedFood.Description;
            existingFood.Price = updatedFood.Price;
            existingFood.Quantity = updatedFood.Quantity;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Update successful" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        // PUT: api/Foods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutFood(int id, Food food)
        {
            if (id != food.Id)
            {
                return BadRequest();
            }

            _context.Entry(food).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(id))
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

        // POST: api/Foods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Food>> PostFood(Food food)
        {
          if (_context.Foods == null)
          {
              return Problem("Entity set 'FoodStoreContext.Foods'  is null.");
          }
            _context.Foods.Add(food);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFood", new { id = food.Id }, food);
        }

        // DELETE: api/Foods/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFood(int id)
        {
            if (_context.Foods == null)
            {
                return NotFound();
            }
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FoodExists(int id)
        {
            return (_context.Foods?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
