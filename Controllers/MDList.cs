using bshbbackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bshbbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MDListController : ControllerBase
    {
        private readonly BshbDbContext _context;

        public MDListController(BshbDbContext context)
        {
            _context = context;
        }

        // GET: api/MDList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MDList>>> GetMDLists()
        {
            return await _context.MDLists.ToListAsync();
        }

        // POST: api/MDList
        [HttpPost]
        public async Task<ActionResult<MDList>> PostMDList(MDList mdList)
        {
            _context.MDLists.Add(mdList);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMDListById), new { id = mdList.id }, mdList);
        }

        // GET: api/MDList/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MDList>> GetMDListById(int id)
        {
            var mdList = await _context.MDLists.FindAsync(id);
            if (mdList == null)
            {
                return NotFound();
            }
            return Ok(mdList);
        }
    }
}
