using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BCrypt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.infrastructure.Persistance;

namespace webapi.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController (IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        // GET: api/users
        [HttpGet]
        public ActionResult GetUsers ([FromQuery] Pagination pagination) {
            var users = _unitOfWork.Users.GetAll ();

            return Ok (PaginatedList<User>.Create (users, pagination.page, pagination.offset));
        }
        // GET: api/users/5
        [HttpGet ("{id}")]
        public ActionResult GetUser (int id) {
            var user = _unitOfWork.Users.GetBy (id);

            if (user == null) {
                return NotFound ();
            }

            return Ok (new { success = true, user });
        }
        // PUT: api/users/5
        [HttpPut ("{id}")]
        public ActionResult PutUser (int id, EditUser values) {
            var user = _unitOfWork.Users.GetBy (id);
            if (user == null) {
                return NotFound ();
            }
            user.Email = values.email;

            _unitOfWork.Complete ();

            return Ok (new { success = true });
        }

        // POST: api/users
        [HttpPost]
        public ActionResult PostUser (User user) {
            _unitOfWork.Users.Add (user);

            var isDuplicateEmail = _unitOfWork.Users.Find (e =>
                e.Email == user.Email
            ).Count ();

            user.Password = BCrypt.Net.BCrypt.HashPassword (user.Password);

            if (isDuplicateEmail > 0) {
                return BadRequest (new { success = false, message = "Email is unique" });
            }

            _unitOfWork.Complete ();
            return Ok (new { success = true, user = user });
        }
        // DELETE: api/users/5
        [HttpDelete ("{id}")]
        public ActionResult DeleteUser (int id) {
            var user = _unitOfWork.Users.GetBy (id);

            if (user == null) {
                return NotFound ();
            }

            _unitOfWork.Users.Remove (user);
            _unitOfWork.Complete ();

            return Ok (new { success = true });
        }
    }
}