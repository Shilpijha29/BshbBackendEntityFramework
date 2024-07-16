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
    public class ContactListController : ControllerBase
    {
        private readonly BshbDbContext _context;

        public ContactListController(BshbDbContext context)
        {
            _context = context;
        }

        // GET: api/ContactList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactList>>> GetContactLists()
        {
            return await _context.ContactLists.ToListAsync();
        }

        // GET: api/ContactList/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactList>> GetContactListById(int id)
        {
            var contactList = await _context.ContactLists.FindAsync(id);
            if (contactList == null)
            {
                return NotFound();
            }
            return contactList;
        }

        // POST: api/ContactList
        [HttpPost]
        public async Task<ActionResult<ContactList>> PostContactList(ContactList contactList)
        {
            _context.ContactLists.Add(contactList);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContactListById), new { id = contactList.Id }, contactList);
        }

        // PUT: api/ContactList/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactList(int id, ContactList contactList)
        {
            if (id != contactList.Id)
            {
                return BadRequest();
            }

            _context.Entry(contactList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactListExists(id))
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

        // DELETE: api/ContactList/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactList(int id)
        {
            var contactList = await _context.ContactLists.FindAsync(id);
            if (contactList == null)
            {
                return NotFound();
            }

            _context.ContactLists.Remove(contactList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactListExists(int id)
        {
            return _context.ContactLists.Any(e => e.Id == id);
        }
    }
}
