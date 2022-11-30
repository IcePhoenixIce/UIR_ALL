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
    public class RecordCurrentsController : ControllerBase
    {
        private readonly UirDbContext _context;

        public RecordCurrentsController(UirDbContext context)
        {
            _context = context;
        }

        // GET: api/RecordCurrents/user/5
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<RecordCurrent>>> GetRecordCurrent(int id)
        {
          if (_context.RecordCurrents == null)
          {
              return NotFound();
          }
            var recordCurrent = await _context.RecordCurrents
                .Include(rec => rec.InvitesCurrent)
                    .ThenInclude(inv=> inv.UserUir)
                .Where(rec => rec.UserUirId == id)
                .ToListAsync();

            if (recordCurrent == null)
            {
                return NotFound();
            }

            return recordCurrent;
        }

        // POST: api/RecordCurrents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RecordCurrent>> PostRecordCurrent(RecordCurrent recordCurrent)
        {
          if (_context.RecordCurrents == null)
          {
              return Problem("Entity set 'UirDbContext.RecordCurrents'  is null.");
          }
            bool fl = false;
            if (recordCurrent.From1.TimeOfDay >= recordCurrent.To1.TimeOfDay)
                return BadRequest(-1);
            var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            var roomView = await _context.Rooms
                .Include(rec => rec.Area)
                .Where(rec => rec.RoomId == recordCurrent.RoomId).FirstOrDefaultAsync();
            if (!(roomView.Area.From1 <= recordCurrent.From1.TimeOfDay && roomView.Area.To1 >= recordCurrent.To1.TimeOfDay))
                return BadRequest(1);
            var recView = await _context.RecordCurrents
                .Include(rec => rec.Room)
                .Where(rec => rec.UserUirId == recordCurrent.UserUirId).ToListAsync();
            foreach (var i in recView)
            {
                if (i.From1.Date == recordCurrent.From1.Date)
                    if (!(i.From1.TimeOfDay >= recordCurrent.To1.TimeOfDay &&
                        i.To1.TimeOfDay <= recordCurrent.From1.TimeOfDay))
                    {
                        return BadRequest(2);
                    }
            }
            var invView = await _context.InvitesCurrents
                .Include(inv => inv.Record)
                .Where(inv => inv.UserUirId == recordCurrent.UserUirId).ToListAsync();
            foreach (var i in invView)
            {
                if (i.Record.From1.Date == recordCurrent.From1.Date)
                    if (!(i.Record.From1.TimeOfDay >= recordCurrent.To1.TimeOfDay &&
                        i.Record.To1.TimeOfDay <= recordCurrent.From1.TimeOfDay))
                    {
                        return BadRequest(3);
                    }
            }
            _context.RecordCurrents.Add(recordCurrent);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return CreatedAtAction("GetRecordCurrent", new { id = recordCurrent.RecordId }, recordCurrent);
        }

        // DELETE: api/RecordCurrents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecordCurrent(int id)
        {
            if (_context.RecordCurrents == null)
            {
                return NotFound();
            }
            var recordCurrent = await _context.RecordCurrents.FindAsync(id);
            if (recordCurrent == null)
            {
                return NotFound();
            }

            _context.RecordCurrents.Remove(recordCurrent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/RecordCurrents/user/5
        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteRecordCurrentUser(int id)
        {
            if (_context.RecordCurrents == null)
            {
                return NotFound();
            }
            var recordCurrents = await _context.RecordCurrents.Where(rec => rec.UserUirId == id).ToListAsync();
            if (recordCurrents == null)
            {
                return NotFound();
            }
            foreach(var i in recordCurrents)
                _context.RecordCurrents.Remove(i);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecordCurrentExists(int id)
        {
            return (_context.RecordCurrents?.Any(e => e.RecordId == id)).GetValueOrDefault();
        }
    }
}
