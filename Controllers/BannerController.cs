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
    public class BannerController : ControllerBase
    {
        private readonly BshbDbContext _context;

        public BannerController(BshbDbContext context)
        {
            _context = context;
        }

        // GET: api/Banner
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Banner>>> GetBanners()
        {
            return await _context.banners.ToListAsync();
        }

        // GET: api/Banner/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Banner>> GetBanner(int id)
        {
            var banner = await _context.banners.FindAsync(id);

            if (banner == null)
            {
                return NotFound();
            }

            return banner;
        }

        // POST: api/Banner
        [HttpPost]
        public async Task<ActionResult<Banner>> CreateBanner([FromForm] BannerDto bannerDto)
        {
            var banner = new Banner
            {
                Photo = await ConvertToByteArrayAsync(bannerDto.Img)
            };

            _context.banners.Add(banner);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBanner), new { id = banner.Id }, banner);
        }

        // PUT: api/Banner/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBanner(int id, [FromForm] BannerDto bannerDto)
        {
            var banner = await _context.banners.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }

            banner.Photo = await ConvertToByteArrayAsync(bannerDto.Img);

            _context.Entry(banner).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Banner/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            var banner = await _context.banners.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }

            _context.banners.Remove(banner);
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
