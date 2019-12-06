using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.core.UseCases;
using webapi.Interfaces;
using webapi.Services;

namespace webapi.Controllers {
  [Authorize]
  [ApiController]
  public class AuthController : ControllerBase {
    private readonly IAuthService _service;

    public AuthController (IAuthService service) {
      _service = service;
    }

    /** /api/auth/login */
    [Route ("/api/auth/login")]
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Login ([FromBody] Login data) {
      return await _service.Login (data);
    }

    // POST: api/auth/me
    [Route ("/api/auth/me")]
    [HttpPost]
    public async Task<ActionResult> GetMeAsync () {
      var currentUserId = int.Parse (User.Identity.Name);
      return await _service.GetMeAsync (currentUserId);
    }

    // PUT: api/auth/changepassword
    [Route ("/api/auth/changepassword")]
    [HttpPut]
    public async Task<ActionResult> ChangeUserPasswordAsync (
      [FromBody] ChangePassword passwords
    ) {
      var currentUserId = int.Parse (User.Identity.Name);
      return await _service.ChangeUserPasswordAsync (
        currentUserId,
        passwords.oldPassword,
        passwords.newPassword);
    }

    // POST: api/auth/changepassword
    [Route ("/api/auth/forgotpassword")]
    [HttpPost]
    [AllowAnonymous]
    public ActionResult ForgotUserPassoword () {
      SendMailService.SendMail(new Models.Email {
        Message = "Ninh dep trai",
        Subject = "this is subject",
        ToEmail = "ninhnguyen375@gmail.com",
        ToName = "Ninh"
      });
      return Ok();
    }
  }
}