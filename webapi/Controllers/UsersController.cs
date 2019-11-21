using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.UseCases;
using webapi.Interfaces;
using webapi.Services;

namespace webapi.Controllers {
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class UsersController : CustomeControllerBase {
        private readonly IUserService _service;

        public UsersController (IUserService service) {
            _service = service;
        }

        // GET: api/users
        [HttpGet]
        public ActionResult GetUsers ([FromQuery] Pagination pagination, [FromQuery] SearchUser search) {
            var res = _service.GetUsers (pagination, search, User);
            
            return this.HandleRes (res);
        }

        // GET: api/users/5
        [HttpGet ("{id}")]
        public ActionResult GetUser (int id) {
            var res = _service.GetUser (id);

            return this.HandleRes (res);
        }

        // PUT: api/users/5/block
        [HttpPut ("{id}/block")]
        public ActionResult BlockUser (int id) {
            var res = _service.BlockUser (id);

            return this.HandleRes (res);
        }

        // PUT: api/users/5/unblock
        [HttpPut ("{id}/unblock")]
        public ActionResult UnblockUser (int id) {
            var res = _service.UnBlockUser (id);

            return this.HandleRes (res);
        }

        // PUT: api/users/5
        [HttpPut ("{id}")]
        [Authorize (Roles = "ADMIN, STAFF")]
        public ActionResult PutUser (int id, SaveUserDTO saveUserDTO) {
            var res = _service.PutUser (id, saveUserDTO);

            return this.HandleRes (res);
        }

        // POST: api/users
        [HttpPost]
        [Authorize (Roles = "ADMIN")]
        public ActionResult PostUser ([FromBody] SaveUserDTO saveUserDTO) {
            var res = _service.PostUser (saveUserDTO);

            return this.HandleRes (res);
        }

        // DELETE: api/users/5
        [HttpDelete ("{id}")]
        [Authorize (Roles = "ADMIN")]
        public ActionResult DeleteUser (int id) {
            var res = _service.DeleteUser (id);

            return this.HandleRes (res);
        }
    }
}