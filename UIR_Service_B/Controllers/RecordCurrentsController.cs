using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UIR_Service_B.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static UIR_Service_B.Models.booking;

namespace UIR_Service_B.Controllers
{
	//[Authorize]
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

		// POST: api/RecordCurrents
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost("User")]
		public async Task<ActionResult<RecordCurrent>> PostRecordCurrent(RecordCurrent recordCurrent, int specialistID, decimal price)
		{
			if (_context.RecordCurrents == null)
				return Problem("Entity set 'UirDbContext.RecordCurrents'  is null.");
			if (recordCurrent.From1.TimeOfDay >= recordCurrent.To1.TimeOfDay)
				return BadRequest("Неккоректное время начала, окончания бронирования");
			var roomView = await _context.Rooms
				.Include(rec => rec.Area)
				.Where(rec => rec.RoomId == recordCurrent.RoomId).FirstOrDefaultAsync();
			if (!(roomView.Area.From1 <= recordCurrent.From1.TimeOfDay && roomView.Area.To1 >= recordCurrent.To1.TimeOfDay))
				return BadRequest("В данное время помещение будет закрыто");
            var passGarmony = await _context.PassesGarmony.FindAsync(specialistID);
            if (passGarmony == null)
                return NotFound("Error in log in");

            Dictionary<string, string> data = new Dictionary<string, string>();
            data["email"] = passGarmony.Login;
            data["pass"] = passGarmony.Password;
            data["action"] = "login";
			JsonElement json=await fetch(data);
			string token = json.GetString();
			data.Clear();

			//Проверка баланса
            data["token"] = token; 
            data["action"] = "balance";
            json = await fetch(data);
			decimal balance = decimal.Parse(json.GetProperty("balance").GetString());
			decimal sale = decimal.Parse(json.GetProperty("sale").GetString());
			data.Clear();

			if (balance < price * (1 - sale / 100))
				return Problem("На аккаунте недостаточно средств. Попробуйте чуть позже!");

            //Получение данных о пользователе.
            data["token"] = token;
            data["action"] = "me";
            json = await fetch(data);
            int userID = json.GetProperty("id").GetInt32();
            data.Clear();

            //Бронирование
            data["date"] = recordCurrent.From1.ToShortDateString();
			data["hour"] = recordCurrent.From1.Hour.ToString();
			data["minute"] = recordCurrent.From1.Minute == 0 ? "1/2" : "2/2";
			data["cabinet"] = recordCurrent.RoomId.ToString();
			data["user_id"] = userID.ToString();
			data["token"] = token;
			data["action"] = "lock";
            json = await fetch(data);

			//Добавление в таблицу записи о брони
			int id = json.GetProperty("id").GetInt32();
			recordCurrent.RecordId = id;
			recordCurrent.InvitesCurrents = new List<InvitesCurrent>() { new InvitesCurrent { RecordId = id, UserUirId = specialistID } };
            _context.RecordCurrents.Add(recordCurrent);
            await _context.SaveChangesAsync();
			return recordCurrent;

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
			var recordCurrent = await _context.RecordCurrents.Include(rec => rec.InvitesCurrents).Where(rec => rec.RecordId == id).FirstOrDefaultAsync(); ;
			if (recordCurrent == null)
			{
				return NotFound();
			}

            //Получение токена
            var passGarmony = await _context.PassesGarmony.FindAsync(recordCurrent.InvitesCurrents.First().UserUirId);
            if (passGarmony == null)
                return NotFound("Error in log in");
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["email"] = passGarmony.Login;
            data["pass"] = passGarmony.Password;
            data["action"] = "login";
            JsonElement json = await fetch(data);
            string token = json.GetString();
            data.Clear();

            //Получение данных о пользователе.
            data["token"] = token;
            data["action"] = "me";
            json = await fetch(data);
            int userID = json.GetProperty("id").GetInt32();
            data.Clear();

            //Отмена брони
            data["id"] = id.ToString();
            data["user_id"] = userID.ToString();
            data["token"] = token;
            data["action"] = "unlock";
            json = await fetch(data);
			if (json.GetBoolean()) 
			{
                _context.RecordCurrents.Remove(recordCurrent);
                await _context.SaveChangesAsync();

                return NoContent();
            }
			return Problem("Не получилось отменить запись");

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

        private async Task<JsonElement> fetch(Dictionary<string, string> data)
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
				var stream = await response.Content.ReadAsStreamAsync();
				var document = await JsonDocument.ParseAsync(stream);
				return document.RootElement.GetProperty("result");
			}
			catch (Exception ex)
			{
				return System.Text.Json.JsonSerializer.SerializeToElement(ex.Message);
			}
        }
    }
}
