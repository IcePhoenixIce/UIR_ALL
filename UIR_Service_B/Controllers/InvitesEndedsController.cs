using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UIR_Service_B.Models;

namespace UIR_Service_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitesEndedsController : ControllerBase
    {
        private readonly UirDbContext _context;

        public InvitesEndedsController(UirDbContext context)
        {
            _context = context;
        }

/*        // GET: api/InvitesEndeds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvitesEnded>>> GetInvitesEndeds()
        {
          if (_context.InvitesEndeds == null)
          {
              return NotFound();
          }
            return await _context.InvitesEndeds.ToListAsync();
        }*/

        // GET: api/InvitesEndeds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<InvitesEnded>>> GetInvitesEnded(int recid)
        {
          if (_context.InvitesEndeds == null)
          {
              return NotFound();
          }
            var invitesEnded = await _context.InvitesEndeds
                .Include(inv => inv.Record)
                .Where(inv => inv.RecordId == recid).ToListAsync();

            if (invitesEnded == null)
            {
                return NotFound();
            }

            return invitesEnded;
        }

        // GET: api/InvitesEndeds/5
        [HttpGet("User/{id}")]
        public async Task<ActionResult<IEnumerable<InvitesEnded>>> GetInvitesUserEnded(int userID)
        {
            if (_context.InvitesEndeds == null)
            {
                return NotFound();
            }
            var invitesEnded = await _context.InvitesEndeds
                .Include(inv => inv.Record)
                .Where(inv => inv.UserUirId == userID).ToListAsync();

            if (invitesEnded == null)
            {
                return NotFound();
            }

            return invitesEnded;
        }

        /*        // PUT: api/InvitesEndeds/5
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPut("{id}")]
                public async Task<IActionResult> PutInvitesEnded(int id, InvitesEnded invitesEnded)
                {
                    if (id != invitesEnded.RecordId)
                    {
                        return BadRequest();
                    }

                    _context.Entry(invitesEnded).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!InvitesEndedExists(id))
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

                // POST: api/InvitesEndeds
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPost]
                public async Task<ActionResult<InvitesEnded>> PostInvitesEnded(InvitesEnded invitesEnded)
                {
                  if (_context.InvitesEndeds == null)
                  {
                      return Problem("Entity set 'UirDbContext.InvitesEndeds'  is null.");
                  }
                    _context.InvitesEndeds.Add(invitesEnded);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {
                        if (InvitesEndedExists(invitesEnded.RecordId))
                        {
                            return Conflict();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return CreatedAtAction("GetInvitesEnded", new { id = invitesEnded.RecordId }, invitesEnded);
                }

                // DELETE: api/InvitesEndeds/5
                [HttpDelete("{id}")]
                public async Task<IActionResult> DeleteInvitesEnded(int id)
                {
                    if (_context.InvitesEndeds == null)
                    {
                        return NotFound();
                    }
                    var invitesEnded = await _context.InvitesEndeds.FindAsync(id);
                    if (invitesEnded == null)
                    {
                        return NotFound();
                    }

                    _context.InvitesEndeds.Remove(invitesEnded);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }*/

        private bool InvitesEndedExists(int id)
        {
            return (_context.InvitesEndeds?.Any(e => e.RecordId == id)).GetValueOrDefault();
        }
    }
}
