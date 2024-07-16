using bshbbackend.ModelDto;
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
    public class ChairmanController : ControllerBase
    {
        private readonly BshbDbContext _context;

        public ChairmanController(BshbDbContext context)
        {
            _context = context;
        }

        // GET: api/Chairman
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chairman>>> GetChairmen()
        {
            return await _context.Chairmen.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Chairman>> PostChairman([FromForm] ChairmenDto chairmenDto)
        {
            var chairman = new Chairman
            {
                Name = chairmenDto.Name,
                From = chairmenDto.From,
                To = chairmenDto.To,
                Photo = chairmenDto.photo != null ? ConvertToBytes(chairmenDto.photo) : null
            };

            _context.Chairmen.Add(chairman);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetChairmanById), new { id = chairman.Id }, chairman);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChairman(int id, [FromForm] ChairmenDto chairmenDto)
        {
            var chairman = await _context.Chairmen.FindAsync(id);
            if (chairman == null)
            {
                return NotFound();
            }

            chairman.Name = chairmenDto.Name;
            chairman.From = chairmenDto.From;
            chairman.To = chairmenDto.To;
            if (chairmenDto.photo != null)
            {
                chairman.Photo = ConvertToBytes(chairmenDto.photo);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private byte[] ConvertToBytes(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }


        // GET: api/Chairman/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Chairman>> GetChairmanById(int id)
        {
            var chairman = await _context.Chairmen.FindAsync(id);
            if (chairman == null)
            {
                return NotFound();
            }
            return Ok(chairman);
        }
    }
}
