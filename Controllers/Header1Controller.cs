using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bshbbackend.ModelDto;
using bshbbackend.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace bshbbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Header1Controller : ControllerBase
    {
        private readonly BshbDbContext _context;

        public Header1Controller(BshbDbContext context)
        {
            _context = context;
        }

        // GET: api/Header1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Header1>>> GetHeaders()
        {
            return await _context.header1s.ToListAsync();
        }

        // GET: api/Header1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Header1>> GetHeader(int id)
        {
            var header = await _context.header1s.FindAsync(id);

            if (header == null)
            {
                return NotFound();
            }

            return header;
        }

        // POST: api/Header1
        [HttpPost]
        public async Task<ActionResult<Header1>> CreateHeader([FromForm] Header1Dto headerDto)
        {
            var header = new Header1
            {
                url1 = headerDto.url1,
                Img1 = await ConvertToByteArrayAsync(headerDto.Img1),
                text1 = headerDto.text1,
                text2 = headerDto.text2,
                url2 = headerDto.url2,
                Img2 = await ConvertToByteArrayAsync(headerDto.Img2)
            };

            _context.header1s.Add(header);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHeader), new { id = header.id }, header);
        }

        // PUT: api/Header1/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHeader(int id, [FromForm] Header1Dto headerDto)
        {
            var header = await _context.header1s.FindAsync(id);
            if (header == null)
            {
                return NotFound();
            }

            header.url1 = headerDto.url1;
            header.Img1 = await ConvertToByteArrayAsync(headerDto.Img1);
            header.text1 = headerDto.text1;
            header.text2 = headerDto.text2;
            header.url2 = headerDto.url2;
            header.Img2 = await ConvertToByteArrayAsync(headerDto.Img2);

            _context.Entry(header).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Header1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHeader(int id)
        {
            var header = await _context.header1s.FindAsync(id);
            if (header == null)
            {
                return NotFound();
            }

            _context.header1s.Remove(header);
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
