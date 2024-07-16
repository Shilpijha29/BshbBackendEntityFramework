using bshbbackend.ModelDto;
using bshbbackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bshbbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficerListController : ControllerBase
    {
        private readonly BshbDbContext _context;

        public OfficerListController(BshbDbContext context)
        {
            _context = context;
        }

        // GET: api/OfficerList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfficerList>>> GetOfficerLists()
        {
            return await _context.OfficerLists.ToListAsync();
        }

        // GET: api/OfficerList/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OfficerList>> GetOfficerListById(int id)
        {
            var officerList = await _context.OfficerLists.FindAsync(id);
            if (officerList == null)
            {
                return NotFound();
            }
            return Ok(officerList);
        }

        // POST: api/OfficerList
        [HttpPost]
        public async Task<ActionResult<OfficerList>> PostOfficerList([FromForm] OfficerListDto officerListDto)
        {
            var officerList = new OfficerList
            {
                Name = officerListDto.Name,
                Designation = officerListDto.Designation,
                details = officerListDto.Details,
                Photo = await GetPhotoBytes(officerListDto.Photo)
            };

            _context.OfficerLists.Add(officerList);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOfficerListById), new { id = officerList.Id }, officerList);
        }

        // PUT: api/OfficerList/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOfficerList(int id, [FromForm] OfficerListDto officerListDto)
        {
            if (id != officerListDto.Id)
            {
                return BadRequest();
            }

            var officerList = await _context.OfficerLists.FindAsync(id);
            if (officerList == null)
            {
                return NotFound();
            }

            officerList.Name = officerListDto.Name;
            officerList.Designation = officerListDto.Designation;
            officerList.details = officerListDto.Details;
            officerList.Photo = await GetPhotoBytes(officerListDto.Photo);

            _context.Entry(officerList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfficerListExists(id))
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

        // DELETE: api/OfficerList/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOfficerList(int id)
        {
            var officerList = await _context.OfficerLists.FindAsync(id);
            if (officerList == null)
            {
                return NotFound();
            }

            _context.OfficerLists.Remove(officerList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OfficerListExists(int id)
        {
            return _context.OfficerLists.Any(o => o.Id == id);
        }

        private async Task<byte[]> GetPhotoBytes(IFormFile photo)
        {
            if (photo == null) return null;
            using (var stream = new MemoryStream())
            {
                await photo.CopyToAsync(stream);
                return stream.ToArray();
            }
        }
    }
}
