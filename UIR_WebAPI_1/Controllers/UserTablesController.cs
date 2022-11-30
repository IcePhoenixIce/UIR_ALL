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
    public class UserTablesController : ControllerBase
    {
        private readonly UirDbContext _context;

        public UserTablesController(UirDbContext context)
        {
            _context = context;
        }

        // GET: api/UserTables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTable>>> GetUserTables()
        {
          if (_context.UserTables == null)
          {
              return NotFound();
          }
            return await _context.UserTables.ToListAsync();
        }

        // GET: api/UserTables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTable>> GetUserTable(int id)
        {
          if (_context.UserTables == null)
          {
              return NotFound();
          }
            var userTable = await _context.UserTables
                .Include(user => user.Pass)
                .Where(user => user.UserUirId == id)
                .FirstOrDefaultAsync();

            if (userTable == null)
            {
                return NotFound();
            }

            return userTable;
        }

        // PUT: api/UserTables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTable(int id, UserTable userTable)
        {
            if (id != userTable.UserUirId)
            {
                return BadRequest();
            }

            _context.Entry(userTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTableExists(id))
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

        // POST: api/UserTables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserTable>> PostUserTable(UserTable userTable)
        {
          if (_context.UserTables == null)
          {
              return Problem("Entity set 'UirDbContext.UserTables'  is null.");
          }
            if (userTable.Pass == null)
                return BadRequest();
            _context.UserTables.Add(userTable);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserTable), new { id = userTable.UserUirId }, userTable);
        }

        // DELETE: api/UserTables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTable(int id)
        {
            if (_context.UserTables == null)
            {
                return NotFound();
            }
            var userTable = await _context.UserTables.FindAsync(id);
            if (userTable == null)
            {
                return NotFound();
            }

            _context.UserTables.Remove(userTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserTableExists(int id)
        {
            return (_context.UserTables?.Any(e => e.UserUirId == id)).GetValueOrDefault();
        }
    }
}
