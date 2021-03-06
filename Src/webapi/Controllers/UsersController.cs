﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.UseCases;
using webapi.Interfaces;

namespace webapi.Controllers {
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly IUserService _service;

        public UsersController (IUserService service) {
            _service = service;
        }

        // GET: api/users
        [HttpGet]
        [Authorize (Roles = "ADMIN, STAFF")]
        public async Task<ActionResult> GetUsersAsync ([FromQuery] Pagination pagination, [FromQuery] SearchUser search) {
            var res = await _service.GetUsersAsync (pagination, search, User);
            
            return res;
        }

        // GET: api/users/5
        [Authorize (Roles = "ADMIN, STAFF")]
        [HttpGet ("{id}")]
        public async Task<ActionResult> GetUserAsync (int id) {
            var res = await _service.GetUserAsync (id);

            return res;
        }

        // PUT: api/users/5/block
        [Authorize (Roles = "ADMIN")]
        [HttpPut ("{id}/block")]
        public async Task<ActionResult> BlockUserAsync (int id) {
            var res = await _service.BlockUserAsync (id);

            return res;
        }

        // PUT: api/users/5/unblock
        [Authorize (Roles = "ADMIN")]
        [HttpPut ("{id}/unblock")]
        public async Task<ActionResult> UnblockUserAsync (int id) {
            var res = await _service.UnBlockUserAsync (id);

            return res;
        }

        // PUT: api/users/5
        [HttpPut ("{id}")]
        [Authorize (Roles = "ADMIN, STAFF")]
        public async Task<ActionResult> PutUserAsync (int id, SaveUserDTO saveUserDTO) {
            var res = await _service.UpdateUserAsync (id, saveUserDTO);

            return res;
        }

        // POST: api/users
        [HttpPost]
        [Authorize (Roles = "ADMIN")]
        public async Task<ActionResult> PostUserAsync ([FromBody] SaveUserDTO saveUserDTO) {
            var res = await _service.AddUserAsync (saveUserDTO);

            return res;
        }

        // DELETE: api/users/5
        [HttpDelete ("{id}")]
        [Authorize (Roles = "ADMIN")]
        public async Task<ActionResult> DeleteUserAsync (int id) {
            var res = await _service.DeleteUserAsync (id);

            return res;
        }

        // PUT: api/users/5/resetpassword
        [HttpPut ("{id}/resetpassword")]
        [Authorize (Roles = "ADMIN")]
        public async Task<ActionResult> ResetUserPassword (int id) {
            var res = await _service.ResetUserPassword (id);

            return res;
        }
    }
}