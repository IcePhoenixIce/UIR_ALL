using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UIR_Service_B.Models;
using System.Security.Cryptography;

namespace UIR_Service_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassesController : ControllerBase
    {
        private readonly UirDbContext _context;

        public PassesController(UirDbContext context)
        {
            _context = context;
        }

        /*        // GET: api/Passes/5
                [HttpGet("{id}")]
                public async Task<ActionResult<Pass>> GetPass(int id)
                {
                    if (_context.Passes == null)
                    {
                        return NotFound();
                    }
                    var pass = await _context.Passes.FindAsync(id);

                    if (pass == null)
                    {
                        return NotFound();
                    }

                    return pass;
                }*/

        // PUT: api/Passes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPass(int id, Pass pass)
        {
            if (id != pass.UserUirId)
            {
                return BadRequest();
            }
            byte[] bytes = Encoding.UTF8.GetBytes(pass.PasswordHash);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            pass.PasswordHash = hashString;
            _context.Entry(pass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PassExists(id))
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

        private bool PassExists(int id)
        {
            return (_context.Passes?.Any(e => e.UserUirId == id)).GetValueOrDefault();
        }
    }
}
