using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UIR_Service_B.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UIR_Service_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassGarmoniesController : ControllerBase
    {
        private readonly UirDbContext _context;

        public PassGarmoniesController(UirDbContext context)
        {
            _context = context;
        }

        // GET: api/PassGarmonies/5 email=savdavid2%40gmail.com&pass=IcePhoenix312&action=login
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetPassGarmony(int? id)
        {
          if (_context.PassesGarmony == null)
          {
              return NotFound();
          }
            var passGarmony = await _context.PassesGarmony.FindAsync(id);

            if (passGarmony == null)
            {
                return NotFound();
            }

            using var client = new HttpClient();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["email"] = passGarmony.Login;
            data["pass"] = passGarmony.Password;
            data["action"] = "login";
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
                using var document = await JsonDocument.ParseAsync(stream);
                JsonElement jsonElement = document.RootElement;
                string token = jsonElement.GetProperty("result").GetString();
                return token;
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        private bool PassGarmonyExists(int? id)
        {
            return (_context.PassesGarmony?.Any(e => e.UserUirId == id)).GetValueOrDefault();
        }
    }
}
