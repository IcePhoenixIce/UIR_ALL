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

namespace UIR_Service_B.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<(Room, IDictionary<DateTime, ICollection<RecordService>>)>> GetRoom(int id)
        {
            if (_context.Rooms == null)
            {
                return NotFound();
            }
            var room = await _context.Rooms
                .Include(r => r.RecordCurrents)
                    .ThenInclude(r => r.InvitesCurrents)
                .Where(r => r.RoomId == id)
                .FirstOrDefaultAsync();

            Dictionary<DateTime, ICollection<RecordService>> recordsServices = new Dictionary<DateTime, ICollection<RecordService>>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            DateTime date = DateTime.Now.Date;

            for (int i = 0; i<7; i++) 
            {
                data["date"] = date.ToShortDateString();
                data["office"] = room.AreaId.ToString();
                data["action"] = "prices";
                data["token"] = "null";

                JsonElement jsonElement = await fetch(data, id);
                var abc = jsonElement.GetProperty("prices").Deserialize<List<Price>>();
                List<RecordService> list_records = PricesService1(abc, id, date);
                FindAllBookingForRoom(jsonElement.GetProperty("bookings").Deserialize<List<booking>>(), ref list_records, id);


                recordsServices.Add(date, list_records);
                date = date.AddDays(1);
            }

            return (room, recordsServices);
        }

        private bool InvitesCurrentExists(int id)
        {
            return (_context.InvitesCurrents?.Any(e => e.RecordId == id)).GetValueOrDefault();
        }

        private async Task<JsonElement> fetch(Dictionary<string, string> data, int id)
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

        private List<RecordService> PricesService1(List<Price>list_price, int roomID, DateTime date) 
        {
            List<RecordService> records = new List<RecordService>();
            foreach(Price price in list_price) 
            {
                if (price.CabinetId == roomID) 
                {
                    date = date.AddHours(price.Hour - date.Hour);
                    date = date.AddMinutes(0 - date.Minute);
                    records.Add(new RecordService { RoomID = roomID, From1 = date, To1 = date.AddMinutes(30), Price = price.price });
                    records.Add(new RecordService { RoomID = roomID, From1 = date.AddMinutes(30), To1 = date.AddMinutes(60), Price = price.price });
                }
            }
            return records;
        }

        //Передать сюда уже массив для записей с ценами
        private void FindAllBookingForRoom(List<booking> obj, ref List<RecordService> list_records, int roomID) 
        {
            foreach (booking book in obj)
            {
                if (book.cabinet.id == list_records[0].RoomID)
                    foreach (RecordService record in list_records)
                        if (record.From1 == DateTime.ParseExact(book.beginAt, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)) 
                        {
                            record.IsBooked = true;
                            break;
                        }

            }
        }
    }
}