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
    public class EmployeeListController : ControllerBase
    {
        private readonly BshbDbContext _context;

        public EmployeeListController(BshbDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeList>>> GetEmployeeLists()
        {
            return await _context.EmployeeLists.ToListAsync();
        }

        // GET: api/EmployeeList/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeList>> GetEmployeeListById(int id)
        {
            var employeeList = await _context.EmployeeLists.FindAsync(id);
            if (employeeList == null)
            {
                return NotFound();
            }
            return Ok(employeeList);
        }

        // POST: api/EmployeeList
        [HttpPost]
        public async Task<ActionResult<EmployeeList>> PostEmployeeList(EmployeeList employeeList)
        {
            _context.EmployeeLists.Add(employeeList);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmployeeListById), new { id = employeeList.Id }, employeeList);
        }

        // PUT: api/EmployeeList/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeList(int id, EmployeeList employeeList)
        {
            if (id != employeeList.Id)
            {
                return BadRequest();
            }

            _context.Entry(employeeList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeListExists(id))
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

        // DELETE: api/EmployeeList/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeList(int id)
        {
            var employeeList = await _context.EmployeeLists.FindAsync(id);
            if (employeeList == null)
            {
                return NotFound();
            }

            _context.EmployeeLists.Remove(employeeList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeListExists(int id)
        {
            return _context.EmployeeLists.Any(e => e.Id == id);
        }
    }
}
