using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UIR_WebAPI_1.Models;

namespace UIR_WebAPI_1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentEndedsController : ControllerBase
    {
        private readonly UirDbContext _context;

        public AppointmentEndedsController(UirDbContext context)
        {
            _context = context;
        }

        // GET: api/AppointmentEndeds/User/5
        [HttpGet("User/{id}")]
        public async Task<ActionResult<IEnumerable<AppointmentEnded>>> GetAppointmentEnded(int id)
        {
          if (_context.AppointmentEndeds == null)
          {
              return NotFound();
          }
            return await _context.AppointmentEndeds.Where(ae => ae.UserUirId == id).ToListAsync();
        }

        // GET: api/AppointmentEndeds/Rating/5
        [HttpGet("Rating/{id}")]
        public async Task<ActionResult<double>> GetAppointmentEndedRating(int id)
        {
            if (_context.AppointmentEndeds == null)
            {
                return NotFound();
            }
            var rating = await _context.AppointmentEndeds.Where(ae => ae.SpecialistId == id).AverageAsync(ae => ae.RatingId);
            if (rating == null)
                return NotFound();
            return rating;
        }

        // PUT: api/AppointmentEndeds/User5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("User/{id}")]
        public async Task<IActionResult> PutAppointmentEnded(int id, AppointmentEnded appointmentEnded)
        {
            if (id != appointmentEnded.AppointmentId)
            {
                return BadRequest();
            }

            _context.Entry(appointmentEnded).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentEndedExists(id))
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

        private bool AppointmentEndedExists(int id)
        {
            return (_context.AppointmentEndeds?.Any(e => e.AppointmentId == id)).GetValueOrDefault();
        }
    }
}
