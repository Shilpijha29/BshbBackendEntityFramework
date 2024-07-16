using bshbbackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bshbbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MDMessageController : ControllerBase
    {
        private readonly BshbDbContext _context;

        public MDMessageController(BshbDbContext context)
        {
            _context = context;
        }

        // GET: api/MDMessage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MDMessage>>> GetMDMessages()
        {
            return await _context.MDMessages.ToListAsync();
        }

        // GET: api/MDMessage/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MDMessage>> GetMDMessageById(int id)
        {
            var mdMessage = await _context.MDMessages.FindAsync(id);
            if (mdMessage == null)
            {
                return NotFound();
            }
            return Ok(mdMessage);
        }

        // POST: api/MDMessage
        [HttpPost]
        public async Task<ActionResult<MDMessage>> PostMDMessage(MDMessage mdMessage)
        {
            _context.MDMessages.Add(mdMessage);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMDMessageById), new { id = mdMessage.id }, mdMessage);
        }

        // PUT: api/MDMessage/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMDMessage(int id, MDMessage mdMessage)
        {
            if (id != mdMessage.id)
            {
                return BadRequest();
            }

            _context.Entry(mdMessage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MDMessageExists(id))
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

        // DELETE: api/MDMessage/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMDMessage(int id)
        {
            var mdMessage = await _context.MDMessages.FindAsync(id);
            if (mdMessage == null)
            {
                return NotFound();
            }

            _context.MDMessages.Remove(mdMessage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MDMessageExists(int id)
        {
            return _context.MDMessages.Any(m => m.id == id);
        }
    }
}
