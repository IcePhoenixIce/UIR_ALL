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
    public class SheduleTablesController : ControllerBase
    {
        private readonly UirDbContext _context;

        public SheduleTablesController(UirDbContext context)
        {
            _context = context;
        }

        // GET: api/SheduleTables/id
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<SheduleTable>>> GetSheduleTables(int id)
        {
          if (_context.SheduleTables == null)
          {
              return NotFound();
          }
            var sheduleTables = await _context.SheduleTables.Where(st => st.SpecialistId == id).ToListAsync();

            if (sheduleTables == null)
            {
                return NotFound();
            }

            return sheduleTables;
        }

        // PUT: api/SheduleTables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSheduleTable(int id, SheduleTable sheduleTable)
        {
            if (id != sheduleTable.SpecialistId)
            {
                return BadRequest();
            }

            _context.Entry(sheduleTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SheduleTableExists(id))
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

        // POST: api/SheduleTables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SheduleTable>> PostSheduleTable(SheduleTable sheduleTable)
        {
          if (_context.SheduleTables == null)
          {
              return Problem("Entity set 'UirDbContext.SheduleTables'  is null.");
          }
            _context.SheduleTables.Add(sheduleTable);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SheduleTableExists(sheduleTable.SpecialistId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSheduleTable", new { id = sheduleTable.SpecialistId }, sheduleTable);
        }

        // DELETE: api/SheduleTables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSheduleTable(int id)
        {
            if (_context.SheduleTables == null)
            {
                return NotFound();
            }
            var sheduleTable = await _context.SheduleTables.FindAsync(id);
            if (sheduleTable == null)
            {
                return NotFound();
            }

            _context.SheduleTables.Remove(sheduleTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SheduleTableExists(int id)
        {
            return (_context.SheduleTables?.Any(e => e.SpecialistId == id)).GetValueOrDefault();
        }
    }
}
