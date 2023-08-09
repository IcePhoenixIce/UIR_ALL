using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using UIR_Service_B.Models;
using UIR_Service_B.ServiceBooking;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static UIR_Service_B.Models.booking;

namespace UIR_Service_B.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class RecordCurrentsController : ControllerBase
	{
		private readonly UirDbContext _context;
		static readonly IFormatProvider _ifp = CultureInfo.InvariantCulture;

		public RecordCurrentsController(UirDbContext context)
		{
			_context = context;
		}

		// GET: api/RecordCurrents/user/5
		[HttpGet("User/{id}")]
		public async Task<ActionResult<IEnumerable<RecordCurrent>>> GetRecordCurrent(int id)
		{
		  if (_context.RecordCurrents == null)
		  {
			  return NotFound();
		  }
			var recordCurrent = await _context.RecordCurrents
				.Include(rec => rec.InvitesCurrents)
					.ThenInclude(inv=> inv.UserUir)
				.Include(rec=>rec.Room)
					.ThenInclude(rec=>rec.Area)
				.Where(rec => rec.UserUirId == id)
				.ToListAsync();

			if (recordCurrent == null)
			{
				return NotFound();
			}

			return recordCurrent;
		}

        [HttpGet("Spec/{id}")]
        public async Task<ActionResult<IEnumerable<RecordCurrent>>> GetRecordCurrentSpec(int id)
        {
            if (_context.RecordCurrents == null)
            {
                return NotFound();
            }
            var recordCurrent = await _context.RecordCurrents
                .Include(rec => rec.InvitesCurrents)
                    .ThenInclude(inv => inv.UserUir)
                .Include(rec => rec.Room)
                    .ThenInclude(rec => rec.Area)
                .Where(rec => rec.InvitesCurrents.FirstOrDefault().UserUirId == id)
                .ToListAsync();

            if (recordCurrent == null)
            {
                return NotFound();
            }

            return recordCurrent;
        }

        // POST: api/RecordCurrents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("User")]
		public async Task<IActionResult> PostRecordCurrent(RecordCurrent recordCurrent)
		{
			int specialistID = recordCurrent.InvitesCurrents.FirstOrDefault().UserUirId;
			decimal price = Convert.ToDecimal(recordCurrent.InvitesCurrents.FirstOrDefault().AdditionalInfo);

            if (recordCurrent.From1.TimeOfDay >= recordCurrent.To1.TimeOfDay)
				return BadRequest("Неккоректное время начала, окончания бронирования");
			if (!(recordCurrent.Room.Area.From1 <= recordCurrent.From1.TimeOfDay && recordCurrent.Room.Area.To1 >= recordCurrent.To1.TimeOfDay))
				return BadRequest("В данное время помещение будет закрыто");
            var passGarmony = await _context.PassesGarmony.Where(p => p.UserUirId == specialistID && p.Servis == recordCurrent.Room.Area.Servis).FirstAsync();
            if (passGarmony == null)
                return NotFound("Ошибка авторизации");
			try
			{
				switch (recordCurrent.Room.Area.Servis)
				{
					case "ASH":
						{
							string token = await ASHRequests.ASHToken(passGarmony);
							var (balance, sale) = await ASHRequests.ASHBalance(token);
							if (balance < price * (1 - sale / 100))
								return Problem("На аккаунте недостаточно средств. Попробуйте чуть позже!");
							string userID = await ASHRequests.ASHMe(token);
							DateTime from = recordCurrent.From1;
							DateTime to = from.AddMinutes(30);
							for (; to < recordCurrent.To1; to = to.AddMinutes(30))
							{
								string id = await ASHRequests.ASHLock(token, userID, recordCurrent, from);
								recordCurrent.ServiceRecord = id;
								recordCurrent.InvitesCurrents = new List<InvitesCurrent>() { new InvitesCurrent { UserUirId = specialistID } };
								recordCurrent.Room = null;
								_context.RecordCurrents.Add(recordCurrent);
								await _context.SaveChangesAsync();
							}
						}
						break;
					case "SoulHelp":
						{
							JObject response = await SoulHelp.SoulHelp_RequestApp();
							string appId = response.Value<string>("id");
							string appSecret = response.Value<string>("appSecret");
							response = await SoulHelp.SoulHelpLogin(appId, appSecret, passGarmony);
							string token = response["accessToken"].ToString();
							decimal balance = Convert.ToDecimal(response["settings"]["balance"], _ifp);
							if(balance < price)
								return Problem("На аккаунте недостаточно средств. Попробуйте чуть позже!");
							string id = await SoulHelp.SoulHelpBooking(appId, appSecret, token, recordCurrent);
							
							recordCurrent.ServiceRecord = id;
							recordCurrent.InvitesCurrents = new List<InvitesCurrent>() { new InvitesCurrent { UserUirId = specialistID } };
							recordCurrent.Room = null;
							_context.RecordCurrents.Add(recordCurrent);
							await _context.SaveChangesAsync();
						}
						break;
					default:
						return BadRequest("Неизвестный сервис");
				}
			}
			catch (Exception ex) 
			{
				return BadRequest($"При попытке записи произошла ошибка: {ex.Message}");
			}
			return Ok();
        }

		//Удаление брони
		// DELETE: api/RecordCurrents/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRecordCurrent(int id)
		{
			if (_context.RecordCurrents == null)
			{
				return NotFound();
			}
			var recordCurrent = await _context.RecordCurrents.Include(rec => rec.InvitesCurrents).Include(rec => rec.Room).ThenInclude(r => r.Area).Where(rec => rec.RecordId == id).FirstOrDefaultAsync();
			if (recordCurrent == null)
			{
				return NotFound();
			}
            var passGarmony = await _context.PassesGarmony.Where(p => (int)p.UserUirId == recordCurrent.InvitesCurrents.First().UserUirId && p.Servis == recordCurrent.Room.Area.Servis).FirstAsync();
            if (passGarmony == null)
                return NotFound("Error in log in");
            switch (recordCurrent.Room.Area.Servis) 
			{
				case "ASH":
					{
                        
                        string token = await ASHRequests.ASHToken(passGarmony);
						string userId = await ASHRequests.ASHMe(token);
                        if (await ASHRequests.ASHdelete(token, userId, recordCurrent))
                        {
                            _context.RecordCurrents.Remove(recordCurrent);
                            await _context.SaveChangesAsync();
                            return NoContent();
                        }
                        return Problem("Не получилось отменить запись");
                    }
					break;
				case "SoulHelp":
					{
                        JObject response = await SoulHelp.SoulHelp_RequestApp();
                        string appId = response.Value<string>("id");
                        string appSecret = response.Value<string>("appSecret");
                        response = await SoulHelp.SoulHelpLogin(appId, appSecret, passGarmony);
                        string token = response["accessToken"].ToString();
						if (await SoulHelp.SoulHelpCancel(appId, appSecret, token, recordCurrent.ServiceRecord))
						{
                            _context.RecordCurrents.Remove(recordCurrent);
                            await _context.SaveChangesAsync();
                            return NoContent();
                        }
                        return Problem("Не получилось отменить запись");
                    }
					break;
				default:
					return BadRequest("Не найдено такого сервиса!");

			}

			#region ASH
			//Получение токена
			

			//Получение данных о пользователе.

            //Отмена брони
            
			
			#endregion
		}

		// DELETE: api/RecordCurrents/user/5
		/*[HttpDelete("User/{id}")]
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
		}*/

		private bool RecordCurrentExists(int id)
		{
			return (_context.RecordCurrents?.Any(e => e.RecordId == id)).GetValueOrDefault();
		}

        private async Task<JObject> POSTrequestASH(Dictionary<string, string> data)
        {
            using var client = new HttpClient();
            var content = new FormUrlEncodedContent(data);

            var contentString = await content.ReadAsStringAsync();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://a-n-h.space/wp-admin/admin-ajax.php"),
            };
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/111.0");
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
            request.Headers.Referrer = new Uri("https://a-n-h.space/raspisanie");
            request.Content = content;
            var response = await client.SendAsync(request);
			try
			{
				return JObject.Parse(await response.Content.ReadAsStringAsync());
			}
			catch (Exception ex)
			{
				return JObject.Parse(ex.Message);
			}
        }
    }
}
