using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bshbbackend.Models;
using bshbbackend.ModelDto;

namespace bshbbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoGalleryController : ControllerBase
    {
        private readonly BshbDbContext _context;

        public PhotoGalleryController(BshbDbContext context)
        {
            _context = context;
        }

        // GET: api/PhotoGallery
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhotoGallery>>> GetPhotoGallery()
        {
            return await _context.PhotoGalleries.ToListAsync();
        }

        // GET: api/PhotoGallery/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PhotoGallery>> GetPhotoById(int id)
        {
            var photo = await _context.PhotoGalleries.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            return Ok(photo);
        }

        // POST: api/PhotoGallery
        [HttpPost]
        public async Task<ActionResult<PhotoGallery>> PostPhoto([FromForm] PhotoGalleryDto photoDto)
        {
            var photo = new PhotoGallery
            {
                Photo = await GetPhotoBytes(photoDto.Photo)
            };

            _context.PhotoGalleries.Add(photo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPhotoById), new { id = photo.Id }, photo);
        }

        // PUT: api/PhotoGallery/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhoto(int id, [FromForm] PhotoGalleryDto photoDto)
        {
            if (id != photoDto.Id)
            {
                return BadRequest();
            }

            var photo = await _context.PhotoGalleries.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            photo.Photo = await GetPhotoBytes(photoDto.Photo);

            _context.Entry(photo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoExists(id))
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

        // DELETE: api/PhotoGallery/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var photo = await _context.PhotoGalleries.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            _context.PhotoGalleries.Remove(photo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhotoExists(int id)
        {
            return _context.PhotoGalleries.Any(e => e.Id == id);
        }

        private async Task<byte[]> GetPhotoBytes(IFormFile photo)
        {
            using (var stream = new MemoryStream())
            {
                await photo.CopyToAsync(stream);
                return stream.ToArray();
            }
        }
    }
}
