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
    public class InvitesCurrentsController : ControllerBase
    {
        private readonly UirDbContext _context;

        public InvitesCurrentsController(UirDbContext context)
        {
            _context = context;
        }


        // GET: api/InvitesCurrents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<InvitesCurrent>>> GetInvitesCurrent(int id)
        {
            if (_context.InvitesCurrents == null)
            {
                return NotFound();
            }
            var invitesCurrent = await _context.InvitesCurrents.Where(inv => inv.RecordId == id).ToListAsync();

            if (invitesCurrent == null)
            {
                return NotFound();
            }

            return invitesCurrent;
        }

        // POST: api/InvitesCurrents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IEnumerable<InvitesCurrent>>> PostInvitesCurrent(InvitesCurrent invitesCurrent)
        {
            if (_context.InvitesCurrents == null)
            {
                return Problem("Entity set 'UirDbContext.InvitesCurrents'  is null.");
            }
            _context.InvitesCurrents.Add(invitesCurrent);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (InvitesCurrentExists(invitesCurrent.RecordId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetInvitesCurrent), new { id = invitesCurrent.RecordId }, invitesCurrent);
        }

        // DELETE: api/InvitesCurrents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvitesCurrent(int recid, int userid)
        {
            if (_context.InvitesCurrents == null)
            {
                return NotFound();
            }
            var invitesCurrent = await _context.InvitesCurrents.FindAsync(recid, userid);
            if (invitesCurrent == null)
            {
                return NotFound();
            }

            _context.InvitesCurrents.Remove(invitesCurrent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InvitesCurrentExists(int id)
        {
            return (_context.InvitesCurrents?.Any(e => e.RecordId == id)).GetValueOrDefault();
        }
    }
}