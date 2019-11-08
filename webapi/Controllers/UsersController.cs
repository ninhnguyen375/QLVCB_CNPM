using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BCrypt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.infrastructure.Persistance;
using webapi.Services;
namespace webapi.Controllers {
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController (IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        // GET: api/users
        [HttpGet]
        public ActionResult GetUsers ([FromQuery] Pagination pagination, [FromQuery] SearchUser search) {
            var currentUserId = int.Parse (User.Identity.Name);
            var usersQuery = _unitOfWork.Users;
            IEnumerable<User> users = usersQuery.GetAll();

            if(search.email != "") {
                users = users.Where(u => u.Email.Contains(search.email));
            }
            if(search.fullname != "") {
                users = users.Where(u => u.FullName.Contains(search.fullname));
            }
            if(search.identifier != "") {
                users = users.Where(u => u.Identifier.Contains(search.identifier));
            }
            if(search.phone != "") {
                users = users.Where(u => u.Phone.Contains(search.phone));
            }
            
            if (User.IsInRole ("ADMIN"))
                users = users.Where (i => i.Role.Equals ("STAFF"));
            else
                return Forbid ();

            return Ok (PaginatedList<User>.Create (users, pagination.current, pagination.pageSize));
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
        [Authorize (Roles = "ADMIN")]
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
        [Authorize (Roles = "ADMIN")]
        public ActionResult PostUser ([FromBody] AddUser user) {
            if(_unitOfWork.Users.Find(u => u.Identifier.Equals(user.Identifier)).Count() != 0) {
                return BadRequest(new {
                    Identifier = "CMND đã được sử dụng"
                });
            }
            if(_unitOfWork.Users.Find(u => u.Email.Equals(user.Email)).Count() != 0) {
                return BadRequest(new {
                    Email = "Email đã được sử dụng"
                });
            }

            User newUser = new User {
                Email = user.Email,
                FullName = user.FullName,
                Identifier = user.Identifier,
                Phone = user.Phone
            };

            _unitOfWork.Users.Add(newUser);
            string defaultPassword = "12345678";

            newUser.Password = BCrypt.Net.BCrypt.HashPassword (defaultPassword);


            _unitOfWork.Complete ();
            return Ok (new { success = true, user = user });
        }

        // DELETE: api/users/5
        [HttpDelete ("{id}")]
        [Authorize (Roles = "ADMIN")]
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