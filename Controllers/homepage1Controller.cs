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
    public class homepage1Controller : ControllerBase
    {
        private readonly BshbDbContext _context;

        public homepage1Controller(BshbDbContext context)
        {
            _context = context;
        }

        // GET: api/homepage1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<homepage1>>> Gethomepage1s()
        {
            return await _context.homepage1s.ToListAsync();
        }

        // GET: api/homepage1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<homepage1>> Gethomepage1(int id)
        {
            var homepage1 = await _context.homepage1s.FindAsync(id);

            if (homepage1 == null)
            {
                return NotFound();
            }

            return homepage1;
        }

        // POST: api/homepage1
        [HttpPost]
        public async Task<ActionResult<homepage1>> Createhomepage1([FromForm] homepage1Dto homepage1Dto)
        {
            var homepage1 = new homepage1
            {
                Text = homepage1Dto.Text,
                chiefminister = await ConvertToByteArrayAsync(homepage1Dto.chiefminister),
                chiefministerName = homepage1Dto.chiefministerName,
                departmentminister = await ConvertToByteArrayAsync(homepage1Dto.departmentminister),
                departmentministerName = homepage1Dto.departmentministerName
            };

            _context.homepage1s.Add(homepage1);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Gethomepage1), new { id = homepage1.Id }, homepage1);
        }

        // PUT: api/homepage1/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Updatehomepage1(int id, [FromForm] homepage1Dto homepage1Dto)
        {
            var homepage1 = await _context.homepage1s.FindAsync(id);
            if (homepage1 == null)
            {
                return NotFound();
            }

            homepage1.Text = homepage1Dto.Text;
            homepage1.chiefminister = await ConvertToByteArrayAsync(homepage1Dto.chiefminister);
            homepage1.chiefministerName = homepage1Dto.chiefministerName;

            homepage1.departmentminister = await ConvertToByteArrayAsync(homepage1Dto.departmentminister);
            homepage1.departmentministerName = homepage1Dto.departmentministerName;
            _context.Entry(homepage1).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/homepage1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletehomepage1(int id)
        {
            var homepage1 = await _context.homepage1s.FindAsync(id);
            if (homepage1 == null)
            {
                return NotFound();
            }

            _context.homepage1s.Remove(homepage1);
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
