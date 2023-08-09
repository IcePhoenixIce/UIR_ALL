using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UIR_Service_B.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using Microsoft.SqlServer.Server;
using System.Globalization;
using NuGet.Protocol;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static NuGet.Packaging.PackagingConstants;
using UIR_Service_B.ServiceBooking;

namespace UIR_Service_B.Controllers
{
	//[Authorize]
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
			return await _context.Rooms.Include(r => r.Area).Where(r => r.AreaId == id).ToListAsync();
		}

		// GET: api/Rooms/Room/1
		[HttpGet("Room/{id}")]
		public async Task<ActionResult<IEnumerable<RecordService>>> GetRoom(int id)
		{
			if (_context.Rooms == null)
			{
				return NotFound();
			}
			var room = await _context.Rooms
				.Include(r => r.RecordCurrents)
					.ThenInclude(r => r.InvitesCurrents)
				.Include(r=>r.Area)
				.Where(r => r.RoomId == id)
				.FirstOrDefaultAsync();
			
			switch (room.Area.Servis) 
			{
				
				case "ASH" :
					return Ok(await ASHRequests.ASHRecordsServices(id, room.AreaId));
				case "SoulHelp":
					return Ok(await SoulHelp.SoulHelpRecordsServices(room!.Area!.AdditionalInfo, id, room!.AdditionalInformation!));
				default: return BadRequest();
            }
		}

		private bool InvitesCurrentExists(int id)
		{
			return (_context.InvitesCurrents?.Any(e => e.RecordId == id)).GetValueOrDefault();
		}
	}
}