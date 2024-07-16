using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bshbbackend.Models;

namespace bshbbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticeController : ControllerBase
    {
        private readonly BshbDbContext _context;

        public NoticeController(BshbDbContext context)
        {
            _context = context;
        }

        // GET: api/Notice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notice>>> GetNotices()
        {
            return await _context.Notices.ToListAsync();
        }

        // GET: api/Notice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Notice>> GetNotice(int id)
        {
            var notice = await _context.Notices.FindAsync(id);

            if (notice == null)
            {
                return NotFound();
            }

            return notice;
        }

        // PUT: api/Notice/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotice(int id, Notice notice)
        {
            if (id != notice.Id)
            {
                return BadRequest();
            }

            notice.UpdatedOn = DateTime.Now;
            _context.Entry(notice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoticeExists(id))
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

        // POST: api/Notice
        [HttpPost]
        public async Task<ActionResult<Notice>> PostNotice(Notice notice)
        {
            notice.CreatedOn = DateTime.Now;
            notice.UpdatedOn = DateTime.Now;
            _context.Notices.Add(notice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotice", new { id = notice.Id }, notice);
        }

        // DELETE: api/Notice/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotice(int id)
        {
            var notice = await _context.Notices.FindAsync(id);
            if (notice == null)
            {
                return NotFound();
            }

            _context.Notices.Remove(notice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NoticeExists(int id)
        {
            return _context.Notices.Any(e => e.Id == id);
        }
    }
}
