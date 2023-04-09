using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedeSocial.BLL.Models;
using RedeSocial.DOMAIN;

namespace RedeSocial.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileUsersController : ControllerBase
    {
        private readonly RedeSocialContext _context;

        public ProfileUsersController(RedeSocialContext context)
        {
            _context = context;
        }

        // GET: api/ProfileUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfileUser>>> GetprofileUsers()
        {
          if (_context.profileUsers == null)
          {
              return NotFound();
          }
            return await _context.profileUsers.ToListAsync();
        }

        [Authorize]
        // GET: api/ProfileUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileUser>> GetProfileUser(int id)
        {
          if (_context.profileUsers == null)
          {
              return NotFound();
          }
            var profileUser = await _context.profileUsers.FindAsync(id);

            if (profileUser == null)
            {
                return NotFound();
            }

            return profileUser;
        }

        [Authorize]
        // PUT: api/ProfileUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfileUser(int id, ProfileUser profileUser)
        {
            if (id != profileUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(profileUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileUserExists(id))
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

        // POST: api/ProfileUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProfileUser>> PostProfileUser(ProfileUser profileUser)
        {
          if (_context.profileUsers == null)
          {
              return Problem("Entity set 'RedeSocialContext.profileUsers'  is null.");
          }
            _context.profileUsers.Add(profileUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfileUser", new { id = profileUser.Id }, profileUser);
        }

        [Authorize]
        // DELETE: api/ProfileUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfileUser(int id)
        {
            if (_context.profileUsers == null)
            {
                return NotFound();
            }
            var profileUser = await _context.profileUsers.FindAsync(id);
            if (profileUser == null)
            {
                return NotFound();
            }

            _context.profileUsers.Remove(profileUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProfileUserExists(int id)
        {
            return (_context.profileUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
