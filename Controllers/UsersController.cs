using System;
using BCrypt;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly ApplicationDbContext _context;

        public UsersController (ApplicationDbContext context) {
            _context = context;
        }
        // GET: api/users
        [HttpGet]
        public async Task<ActionResult> GetUsers (
            [FromQuery] Search search, [FromQuery] Pagination pagination
        ) {
            /** Get query */
            var query = _context.Users;

            /** Get Search Data */
            var data = query.Where (e =>
                // Search by name
                e.name.Contains (search.name) &&
                // Search by address
                e.address.Contains (search.address) &&
                // Search by email
                e.email.Contains (search.email) &&
                // Search by gender
                (search.gender == -1 ? true : e.gender == search.gender) &&
                // Search by phone
                e.phone.Contains (search.phone)
            );

            /** total number of user */
            int totalCount = data.Count ();

            /** get page */
            int page = pagination.page;

            /** get limit record in page */
            int limit = pagination.limit;

            /** get totalPage */
            int totalPage = (int) Math.Ceiling (totalCount / (double) limit);

            /** Allmost done, get result in page */
            var result = await data
                .Skip ((page - 1) * limit)
                .Take (limit)
                .ToListAsync ();

            /** total number of user in current page */
            var count = data.Count ();

            return Ok (new {
                success = true,
                    page,
                    limit,
                    totalPage,
                    totalCount,
                    count,
                    data = result,
            });
        }

        // GET: api/users/5
        [HttpGet ("{id}")]
        public async Task<ActionResult> GetUser (long id) {
            var user = await _context.Users.FindAsync (id);

            if (user == null) {
                return NotFound ();
            }

            return Ok (new { success = true, user });
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult> PostUser (User user) {
            _context.Users.Add (user);

            var isDuplicateEmail = _context.Users.Where (e =>
                e.email == user.email
            ).Count();

            user.password = BCrypt.Net.BCrypt.HashPassword(user.password);
            var verify = BCrypt.Net.BCrypt.Verify("12345678", user.password);

            if (isDuplicateEmail > 0) {
                return BadRequest (new { success = false, message = "Email is unique" });
            }

            user.createdAt = DateTime.Now;

            await _context.SaveChangesAsync ();
            // return Created("/api/users", user);
            return Ok (new { success = true, user = user, verify });
        }

        // PUT: api/users/5
        [HttpPut ("{id}")]
        public async Task<ActionResult> PutUser (long id, User item) {
            if (id != item.id) {
                return BadRequest ();
            }

            item.updatedAt = DateTime.Now;

            _context.Entry (item).State = EntityState.Modified;
            await _context.SaveChangesAsync ();

            return Ok (new { success = true, user = item });
        }

        // DELETE: api/users/5
        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteUser (long id) {
            var user = await _context.Users.FindAsync (id);

            if (user == null) {
                return NotFound ();
            }

            _context.Users.Remove (user);
            await _context.SaveChangesAsync ();

            return Ok (new { success = true });
        }
    }
}