using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using BE.Helpers;
using BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BE.Controllers {
  [ApiController]
  public class AuthController : ControllerBase {
    private readonly ApplicationDbContext _context;
    private readonly AppSettings _appSettings;

    public AuthController (ApplicationDbContext context, IOptions<AppSettings> appSettings) {
      _context = context;
      _appSettings = appSettings.Value;
    }

    /** /api/auth/login */
    [Route ("/api/auth/login")]
    [HttpPost]
    public ActionResult Login ([FromBody] LoginData data) {
      var query = _context.Users;
      string email = data.email;
      string password = data.password;

      /** find user valid with email */
      User user = query.SingleOrDefault (i => i.email.Equals (email));

      if (user == null) {
        return BadRequest (new { success = false, message = "User not found" });
      }

      /** compare hashed password with password from body */
      bool verify = BCrypt.Net.BCrypt.Verify (password, user.password);

      if (verify == false) {
        return BadRequest (new { success = false, message = "User name or password incorect" });
      }

      /** render jwt */
      var tokenHandler = new JwtSecurityTokenHandler ();
      var key = Encoding.ASCII.GetBytes (_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor {
        Subject = new ClaimsIdentity (new Claim[] {
        new Claim ("id", user.id.ToString ())
        }),
        Expires = DateTime.UtcNow.AddDays (7),
        SigningCredentials = new SigningCredentials (new SymmetricSecurityKey (key), SecurityAlgorithms.HmacSha256Signature),
      };
      var token = tokenHandler.CreateToken (tokenDescriptor);
      var writedToken = tokenHandler.WriteToken (token);

      // remove password before returning
      user.password = null;

      return Ok (new {
        success = true,
          user,
          token = writedToken,
      });
    }
  }
}