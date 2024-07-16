using bshbbackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bshbbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrentTendersController : ControllerBase
    {
        private readonly BshbDbContext _context;

        public CurrentTendersController(BshbDbContext context)
        {
            _context = context;
        }

        // GET: api/CurrentTenders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrentTenders>>> GetCurrentTenders()
        {
            return await _context.CurrentTender.ToListAsync();
        }

        // GET: api/CurrentTenders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CurrentTenders>> GetCurrentTenderById(int id)
        {
            var currentTender = await _context.CurrentTender.FindAsync(id);
            if (currentTender == null)
            {
                return NotFound();
            }
            return Ok(currentTender);
        }

        // POST: api/CurrentTenders
        [HttpPost]
        public async Task<ActionResult<CurrentTenders>> PostCurrentTender(CurrentTenders currentTender)
        {
            _context.CurrentTender.Add(currentTender);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCurrentTenderById), new { id = currentTender.Id }, currentTender);
        }

        // PUT: api/CurrentTenders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurrentTender(int id, CurrentTenders currentTender)
        {
            if (id != currentTender.Id)
            {
                return BadRequest();
            }

            _context.Entry(currentTender).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurrentTenderExists(id))
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

        // DELETE: api/CurrentTenders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurrentTender(int id)
        {
            var currentTender = await _context.CurrentTender.FindAsync(id);
            if (currentTender == null)
            {
                return NotFound();
            }

            _context.CurrentTender.Remove(currentTender);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CurrentTenderExists(int id)
        {
            return _context.CurrentTender.Any(e => e.Id == id);
        }
    }
}
