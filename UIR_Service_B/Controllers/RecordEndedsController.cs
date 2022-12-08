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
    public class RecordEndedsController : ControllerBase
    {
        private readonly UirDbContext _context;

        public RecordEndedsController(UirDbContext context)
        {
            _context = context;
        }

        //get all for userId
        // GET: api/RecordEndeds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<RecordEnded>>> GetRecordEnded(int id)
        {
          if (_context.RecordEndeds == null)
          {
              return NotFound();
          }
            var recordsEnded = await _context.RecordEndeds
                .Include(rec => rec.InvitesEnded)
                .Where(rec => rec.UserUirId == id)
                .ToListAsync();

            if (recordsEnded == null)
            {
                return NotFound();
            }

            return recordsEnded;
        }

        //get rating for roomID
        // GET: api/RecordEndeds/Room/5
        [HttpGet("Room/{id}")]
        public async Task<ActionResult<double>> GetRecordEndedRoomRating(int id)
        {
            if (_context.RecordEndeds == null)
            {
                return NotFound();
            }
            var rating = await _context.RecordEndeds
                .Where(rec => rec.RoomId == id)
                .AverageAsync(rec => rec.RatingId);
            if (rating == null)
            {
                return NotFound();
            }

            return rating;
        }

        // PUT: api/RecordEndeds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecordEnded(int id, RecordEnded recordEnded)
        {
            if (id != recordEnded.RecordId)
            {
                return BadRequest();
            }

            _context.Entry(recordEnded).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecordEndedExists(id))
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

        private bool RecordEndedExists(int id)
        {
            return (_context.RecordEndeds?.Any(e => e.RecordId == id)).GetValueOrDefault();
        }
    }
}
