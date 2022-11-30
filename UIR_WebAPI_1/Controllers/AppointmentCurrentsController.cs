using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UIR_WebAPI_1.Models;

namespace UIR_WebAPI_1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AppointmentCurrentsController : ControllerBase
	{
		private readonly UirDbContext _context;

		public AppointmentCurrentsController(UirDbContext context)
		{
			_context = context;
		}
        // GET: api/AppointmentCurrents/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentCurrent>> GetAppointmentCurrent(int id)
        {
            if (_context.AppointmentCurrents == null)
            {
                return NotFound();
            }
            var appointmentCurrents = await _context.AppointmentCurrents.FindAsync(id);

            if (appointmentCurrents == null)
            {
                return NotFound();
            }

            return appointmentCurrents;
        }

        // GET: api/AppointmentCurrents/User/5
        [HttpGet("User/{id}")]
		public async Task<ActionResult<IEnumerable<AppointmentCurrent>>> GetAppointmentCurrentUser(int id)
		{
		  if (_context.AppointmentCurrents == null)
		  {
			  return NotFound();
		  }
			var appointmentCurrents = await _context.AppointmentCurrents.Where(ac => ac.UserUirId == id).ToListAsync();

			if (appointmentCurrents == null)
			{
				return NotFound();
			}

			return appointmentCurrents;
		}

		[HttpGet("Specialist/{id}")]
		public async Task<ActionResult<IEnumerable<AppointmentCurrent>>> GetAppointmentCurrentSpec(int id)
		{
			if (_context.AppointmentCurrents == null)
			{
				return NotFound();
			}
			var appointmentCurrents = await _context.AppointmentCurrents.Where(ac => ac.SpecialistId == id).ToListAsync();

			if (appointmentCurrents == null)
			{
				return NotFound();
			}

			return appointmentCurrents;
		}

		// POST: api/AppointmentCurrents
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<AppointmentCurrent>> PostAppointmentCurrent(AppointmentCurrent appointmentCurrent)
		{
		  if (_context.AppointmentCurrents == null)
		  {
			  return Problem("Entity set 'UirDbContext.AppointmentCurrents'  is null.");
		  }
			bool fl=true;

            var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            if (appointmentCurrent.From1.TimeOfDay >= appointmentCurrent.To1.TimeOfDay)
                return BadRequest(-1);
            var SpecView = await _context.Specialists
                .Include(spec => spec.SheduleTables)
                .Include(spec => spec.AppointmentCurrents)
                .Where(spec => spec.SpecialistId == appointmentCurrent.SpecialistId).ToListAsync();
            if (SpecView == null)
                return BadRequest(1);
            foreach (var i in SpecView[0].SheduleTables)
            {
                if (i.WeekdayId == (int)appointmentCurrent.From1.DayOfWeek)
                    if ((i.From1 <= appointmentCurrent.From1.TimeOfDay &&
                        i.LunchStart >= appointmentCurrent.To1.TimeOfDay) ||
                        (i.LunchEnd <= appointmentCurrent.From1.TimeOfDay &&
                        i.To1 >= appointmentCurrent.To1.TimeOfDay))
                    {
                        fl = false;
                        break;
                    }
            }
            if (fl)
                return BadRequest(2);
            foreach (var i in SpecView[0].AppointmentCurrents)
            {
                if (i.From1.Date == appointmentCurrent.From1.Date)
                    if (!(i.To1.TimeOfDay <= appointmentCurrent.From1.TimeOfDay ||
                        i.From1.TimeOfDay >= appointmentCurrent.To1.TimeOfDay))
                        fl = true;
            }
            if (fl)
                return BadRequest(3);
            await _context.AppointmentCurrents.AddAsync(appointmentCurrent);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return CreatedAtAction(nameof(GetAppointmentCurrent), new { id = appointmentCurrent.AppointmentId }, appointmentCurrent);
        }

		// DELETE: api/AppointmentCurrents/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAppointmentCurrentUser(int id)
		{
			if (_context.AppointmentCurrents == null)
			{
				return NotFound();
			}
			var appointmentCurrent = await _context.AppointmentCurrents.FindAsync(id);
			if (appointmentCurrent == null)
			{
				return NotFound();
			}

			_context.AppointmentCurrents.Remove(appointmentCurrent);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		//Если заболел и не может прийти, то все записи на ближайшую неделю отменяются
		[HttpDelete("Specialist/{id}")]
		public async Task<IActionResult> DeleteAppointmentCurrentSpec(int id)
		{
			if (_context.AppointmentCurrents == null)
			{
				return NotFound();
			}
			var appointmentCurrents = await _context.AppointmentCurrents.Where(acs=> acs.SpecialistId == id).ToListAsync();
			if (appointmentCurrents == null)
			{
				return NotFound();
			}
			foreach(var i in appointmentCurrents)
				_context.AppointmentCurrents.Remove(i);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool AppointmentCurrentExists(int id)
		{
			return (_context.AppointmentCurrents?.Any(e => e.AppointmentId == id)).GetValueOrDefault();
		}
	}
}
