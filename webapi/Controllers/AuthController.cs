using System;
using System.Collections;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;

namespace webapi.Controllers {
  [ApiController]
  public class AuthController : ControllerBase {
    private readonly IUnitOfWork _unitOfWork;

    public AuthController (IUnitOfWork unitOfWork) {
      _unitOfWork = unitOfWork;
    }

    /** /api/auth/login */
    [Route ("/api/auth/login")]
    [HttpPost]
    public ActionResult Login ([FromBody] Login data) {
      var query = _unitOfWork.Users;
      string email = data.email;
      string password = data.password;

      /** find user valid with email */
      var user = query.Find (i => i.Email.Equals (email)).SingleOrDefault ();

      if (user == null) {
        return BadRequest (new { success = false, message = "User not found" });
      }

      /** compare hashed password with password from body */
      bool verify = BCrypt.Net.BCrypt.Verify (password, user.Password);

      if (user == null) {
        return BadRequest (new { success = false, message = "User name or password incorect" });
      }

      // remove password before returning
      user.Password = null;

      return Ok (new {
        success = true,
          user,
          // token = writedToken,
      });
    }

    // POST: api/user/me
    [Route ("/api/auth/me")]
    [HttpPost]
    public ActionResult GetMe ([FromBody] Login data) {
      var user = _unitOfWork.Users.Find (u => u.Email.Equals (data.email) && u.Password.Equals (data.password)).SingleOrDefault ();

      if (user == null) {
        return NotFound ();
      }

      return Ok (new { success = true, user });
    }
  }
}