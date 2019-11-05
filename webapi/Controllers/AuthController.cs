using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace webapi.Controllers {
  [Authorize]
  [ApiController]
  public class AuthController : ControllerBase {
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppSettings _appSettings;

    public AuthController (IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings) {
      _unitOfWork = unitOfWork;
      _appSettings = appSettings.Value;
    }

    /** /api/auth/login */
    [Route ("/api/auth/login")]
    [AllowAnonymous]
    [HttpPost]
    public ActionResult Login ([FromBody] Login data) {
      var query = _unitOfWork.Users;
      string email = data.email;
      string password = data.password;

      /** find user valid with email */
      User user = query.Find (i => i.Email.Equals (email)).SingleOrDefault ();

      if (user == null) {
        return BadRequest (new { success = false, message = "User not found" });
      }

      /** compare hashed password with password from body */
      bool verify = BCrypt.Net.BCrypt.Verify (password, user.Password);

      if (verify == false) {
        return BadRequest (new { success = false, message = "User name or password incorect" });
      }

      /** render jwt */
      string tokenString = this.getTokenString(user);

      // remove password before returning
      user.Password = null;

      return Ok (new {
        success = true,
          user,
          token = tokenString,
      });
    }

    // POST: api/user/me
    [Route ("/api/auth/me")]
    [HttpPost]
    public ActionResult GetMe ([FromBody] Login data) {
      var currentUserId = int.Parse (User.Identity.Name);
      User user = _unitOfWork.Users.GetBy(currentUserId);
      if(user == null)
        return Forbid();

      /** render jwt */
      string tokenString = this.getTokenString(user);

      return Ok (new { success = true, user, token = tokenString });
    }

    private string getTokenString (User user) {
      var tokenHandler = new JwtSecurityTokenHandler ();
      var key = Encoding.ASCII.GetBytes (_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor {
        Subject = new ClaimsIdentity (new Claim[] {
        new Claim (ClaimTypes.Name, user.Id.ToString ()),
        new Claim(ClaimTypes.Role, user.Role)
        }),
        Expires = DateTime.UtcNow.AddDays (7),
        SigningCredentials = new SigningCredentials (new SymmetricSecurityKey (key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken (tokenDescriptor);
      return tokenHandler.WriteToken (token);
    }
  }
}