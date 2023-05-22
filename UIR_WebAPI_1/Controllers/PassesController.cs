using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using UIR_WebAPI_1.Models;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using UIR_Service_A.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace UIR_WebAPI_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassesController : ControllerBase
    {
        private readonly UirDbContext _context;
        private readonly JWTSettings _jwtsettings;

        public PassesController(UirDbContext context, IOptions<JWTSettings> jwtsettings)
        {
            _context = context;
            _jwtsettings = jwtsettings.Value;
        }

        // POST: api/Passes/Login
        [HttpPost("Login")]
        public async Task<ActionResult<PassToken>> Login([FromBody] Pass pass)
        {
            if (_context.Passes == null)
            {
                return BadRequest("Invalid client request");
            }
            var pass1 = await _context.Passes
                .Include(p => p.UserUir)
                .ThenInclude(u => u.Specialist)
                .Where(p => p.UserLogin == pass.UserLogin
                     && p.PasswordHash == pass.PasswordHash)
                .FirstOrDefaultAsync();
            PassToken passToken = new PassToken(pass1);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtsettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, pass1.UserLogin)
                }),
                Expires = DateTime.UtcNow.AddMonths(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            passToken.Token = tokenHandler.WriteToken(token);

            if (passToken == null)
            {
                return NotFound();
            }

            return passToken;
        }

        // PUT: api/Passes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPass(int id, Pass pass)
        {
            if (id != pass.UserUirId)
            {
                return BadRequest();
            }

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
