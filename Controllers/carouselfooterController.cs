using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bshbbackend.ModelDto;
using bshbbackend.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace bshbbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class carouselfooterController : ControllerBase
    {
        private readonly BshbDbContext _context;

        public carouselfooterController(BshbDbContext context)
        {
            _context = context;
        }

        // GET: api/carouselfooter
        [HttpGet]
        public async Task<ActionResult<IEnumerable<carouselfooter>>> Getcarouselfooters()
        {
            return await _context.carouselfooters.ToListAsync();
        }

        // GET: api/carouselfooter/5
        [HttpGet("{id}")]
        public async Task<ActionResult<carouselfooter>> Getcarouselfooter(int id)
        {
            var carouselfooter = await _context.carouselfooters.FindAsync(id);

            if (carouselfooter == null)
            {
                return NotFound();
            }

            return carouselfooter;
        }

        // POST: api/carouselfooter
        [HttpPost]
        public async Task<ActionResult<carouselfooter>> Createcarouselfooter([FromForm] carouselfooterDto carouselfooterDto)
        {
            var carouselfooter = new carouselfooter
            {
                url = carouselfooterDto.url,
                photo = await ConvertToByteArrayAsync(carouselfooterDto.photo)
            };

            _context.carouselfooters.Add(carouselfooter);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Getcarouselfooter), new { id = carouselfooter.Id }, carouselfooter);
        }

        // PUT: api/carouselfooter/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Updatecarouselfooter(int id, [FromForm] carouselfooterDto carouselfooterDto)
        {
            var carouselfooter = await _context.carouselfooters.FindAsync(id);
            if (carouselfooter == null)
            {
                return NotFound();
            }

            carouselfooter.url = carouselfooterDto.url;
            carouselfooter.photo = await ConvertToByteArrayAsync(carouselfooterDto.photo);

            _context.Entry(carouselfooter).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/carouselfooter/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletecarouselfooter(int id)
        {
            var carouselfooter = await _context.carouselfooters.FindAsync(id);
            if (carouselfooter == null)
            {
                return NotFound();
            }

            _context.carouselfooters.Remove(carouselfooter);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<byte[]> ConvertToByteArrayAsync(IFormFile formFile)
        {
            if (formFile == null)
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                await formFile.CopyToAsync(ms);
                return ms.ToArray();
            }
        }
    }
}
