using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UIR_Service_B.Models;

namespace UIR_Service_B.Controllers
{
    [Authorize]
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
            try 
            {
                var rec = await _context.RecordCurrents
                    .SingleAsync(rec => rec.RecordId == invitesCurrent.RecordId);
                if (!(rec.From1.TimeOfDay >= rec.To1.TimeOfDay))
                    return BadRequest("From > To");
                var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
                var userInfo = await _context.UserTables
                    .Include(us => us.RecordCurrents)
                    .Include(us => us.InvitesCurrents)
                        .ThenInclude(inv => inv.Record)
                    .SingleAsync(us => us.UserUirId == invitesCurrent.UserUirId);

                foreach (var i in userInfo.RecordCurrents)
                {
                    if (i.From1.Date == rec.From1.Date)
                        if (!(i.From1.TimeOfDay >= rec.To1.TimeOfDay &&
                            i.To1.TimeOfDay <= rec.From1.TimeOfDay))
                        {
                            return BadRequest("User have a room on this time");
                        }
                }
                foreach (var i in userInfo.InvitesCurrents)
                {
                    if (i.Record.From1.Date == rec.From1.Date)
                        if (!(i.Record.From1.TimeOfDay >= rec.To1.TimeOfDay &&
                            i.Record.To1.TimeOfDay <= rec.From1.TimeOfDay))
                        {
                            return BadRequest("User have another invite on this time");
                        }
                }
                _context.InvitesCurrents.Add(invitesCurrent);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
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