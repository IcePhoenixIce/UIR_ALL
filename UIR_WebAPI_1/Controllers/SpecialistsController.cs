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
    public class SpecialistsController : ControllerBase
    {
        private readonly UirDbContext _context;

        public SpecialistsController(UirDbContext context)
        {
            _context = context;
        }

        // GET: api/Specialists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Specialist>>> GetSpecialists()
        {
          if (_context.Specialists == null)
          {
              return NotFound();
          }
            return await _context.Specialists.Include(user => user.SpecialistNavigation).ToListAsync();
        }

        // GET: api/Specialists/4
        [HttpGet("{id}")]
        public async Task<ActionResult<Specialist>> GetSpecialist(int id)
        {
          if (_context.Specialists == null)
          {
              return NotFound();
          }
            var specialist = await _context.Specialists
                .Include(user => user.SpecialistNavigation)
                /*.Include(specialist => specialist.SheduleTables)
                .Include(specialist => specialist.AppointmentCurrents)*/
                .Include(spec => spec.Rooms)
                .Where(specialist => specialist.SpecialistId == id)
                .FirstOrDefaultAsync();

            if (specialist == null)
            {
                return NotFound();
            }

            return specialist;
        }

        // GET: api/Specialists/Shedule/4
        [HttpGet("Shedule/{id}")]
        public async Task<ActionResult<(Specialist, IDictionary<DateTime, IEnumerable<RecordService>>)>> GetAppointmentCurrentSpec(int id)
        {
            var spec = await _context.Specialists
                .Include(spec => spec.AppointmentCurrents)
                .Include(spec => spec.SheduleTables).Where(spec => spec.SpecialistId == id).FirstOrDefaultAsync();
            if (spec == null)
                return NotFound();
            Dictionary<DateTime, IEnumerable<RecordService>> recordsServices = new Dictionary<DateTime, IEnumerable<RecordService>>();
            DateTime date = DateTime.Now.Date;

            for (int i = 0; i < 7; i++)
            {
                bool flag = false;
                var curShedule = spec.SheduleTables.Where(sh => sh.WeekdayId == date.DayOfWeek).FirstOrDefault();
                if (curShedule == null)
                {
                    curShedule = new SheduleTable()
                    {
                        From1 = new TimeSpan(10, 0, 0),
                        To1 = new TimeSpan(22, 0, 0),
                        WeekdayId = date.DayOfWeek,
                        LunchStart = new TimeSpan(13, 0, 0),
                        LunchEnd = new TimeSpan(13, 30, 0),
                        Price = spec.SheduleTables.First().Price,
                        SpecialistId = spec.SpecialistId
                    };
                    flag = true;
                }
                recordsServices.Add(date, CreateShedule(curShedule, date, flag));
                date = date.AddDays(1);
            }
            //Потом уже учет сушествующих записей. Их просчет на основе времени для прямого обращения.
            foreach (var appointment in spec.AppointmentCurrents)
            {
                if (!recordsServices.ContainsKey(appointment.From1.Date))
                    continue;
                foreach (var record in recordsServices[appointment.From1.Date])
                {
                    if (record.From1 == appointment.From1)
                    {
                        record.IsBooked = true;
                        break;
                    }
                }
            }

            return (spec, recordsServices);
        }

        // PUT: api/Specialists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpecialist(int id, Specialist specialist)
        {
            if (id != specialist.SpecialistId)
            {
                return BadRequest();
            }

            _context.Entry(specialist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialistExists(id))
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

        private bool SpecialistExists(int id)
        {
            return (_context.Specialists?.Any(e => e.SpecialistId == id)).GetValueOrDefault();
        }

        private List<RecordService> CreateShedule(SheduleTable shedule, DateTime date, bool flag = false)
        {
            List<RecordService> result = new List<RecordService>();
            TimeSpan buf = new TimeSpan(0, 30, 0);
            for (TimeSpan start = shedule.From1; start < shedule.LunchStart; start = start + buf)
                result.Add(new RecordService() { From1 = date.Add(start), To1 = date.Add(start + buf), IsBooked = false || flag, Price = (decimal)shedule.Price });
            for (TimeSpan start = shedule.LunchStart; start < shedule.LunchEnd; start = start + buf)
                result.Add(new RecordService() { From1 = date.Add(start), To1 = date.Add(start + buf), IsBooked = true || flag, Price = (decimal)shedule.Price });
            for (TimeSpan start = shedule.LunchEnd; start < shedule.To1; start = start + buf)
                result.Add(new RecordService() { From1 = date.Add(start), To1 = date.Add(start + buf), IsBooked = false || flag, Price = (decimal)shedule.Price });
            return result;
        }
    }
}
