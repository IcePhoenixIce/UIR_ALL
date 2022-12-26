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
/*    [Authorize]*/
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly UirDbContext _context;

        public RoomsController(UirDbContext context)
        {
            _context = context;
        }


        // GET: api/Rooms/1
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetAreas(int id)
        {
            if (_context.Rooms == null)
            {
                return NotFound();
            }

            return await _context.Rooms.Where(r => r.AreaId == id).ToListAsync();
        }

        // GET: api/Rooms/Room/1
        [HttpGet("Room/{id}")]
        public async Task<ActionResult<Room>> GetArea(int id)
        {
            if (_context.Rooms == null)
            {
                return NotFound();
            }
            var area = await _context.Rooms
                .Include(r => r.RecordCurrents)
                .Where(r => r.RoomId == id)
                .FirstOrDefaultAsync();
            return area;
        }

        private bool InvitesCurrentExists(int id)
        {
            return (_context.InvitesCurrents?.Any(e => e.RecordId == id)).GetValueOrDefault();
        }
    }
}