using System.Net.Mail;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Helpers;
using webapi.Interfaces;
using webapi.Models;

namespace webapi.Services
{
  public class AuthService : ControllerBase, IAuthService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppSettings _appSettings;
    private readonly IMapper _mapper;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }

    public async Task<ActionResult> Login(Login data)
    {
      var query = _unitOfWork.Users;
      string email = data.email;
      string password = data.password;

      /** find user valid with email */
      // User user = query.Find (i => i.Email.Equals (email)).SingleOrDefault ();
      var userAsync = await query.FindAsync(i => i.Email.Equals(email));
      User user = userAsync.SingleOrDefault();

      if (user == null)
      {
        return BadRequest(new { success = false, message = "Tên tài khoản hoặc mật khẩu không chính xác" });
      }

      /** compare hashed password with password from body */
      bool verify = BCrypt.Net.BCrypt.Verify(password, user.Password);

      if (verify == false)
      {
        return BadRequest(new { success = false, message = "Tên tài khoản hoặc mật khẩu không chính xác" });
      }

      if (user.Status == 2)
      { // 2: banned
        return BadRequest(new { success = false, message = "Tài khoản đã bị khóa" });
      }

      /** render jwt */
      string tokenString = this.getTokenString(user);

      // remove password before returning
      user.Password = null;

      return Ok(new
      {
        success = true,
        user,
        token = tokenString,
      });
    }

    public async Task<ActionResult> ChangeUserPasswordAsync(
      int currentUserId, string oldPassword, string newPassword
    )
    {
      User user = await _unitOfWork.Users.GetByAsync(currentUserId);
      if (user == null || user.Status == 2) // 2: Banned
        return BadRequest(new { message = "Tài khoản không hợp lệ" });

      /** compare hashed password with oldPassword */
      bool verify = BCrypt.Net.BCrypt.Verify(oldPassword, user.Password);

      if (verify == false)
      {
        return BadRequest(new { oldPassword = "Sai mật khẩu" });
      }

      user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

      if (user.Status == 4)
      {
        user.Status = 1; // active
      }

      await _unitOfWork.CompleteAsync();

      return Ok();
    }

    public async Task<ActionResult> GetMeAsync(int currentUserId)
    {
      User user = await _unitOfWork.Users.GetByAsync(currentUserId);
      if (user == null || user.Status == 2) // 2: Banned
        return BadRequest(new { message = "Tài khoản không hợp lệ" });

      /** render jwt */
      string tokenString = this.getTokenString(user);

      return Ok(new { success = true, user, token = tokenString });
    }

    private string getTokenString(User user)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[] {
        new Claim (ClaimTypes.Name, user.Id.ToString ()),
        new Claim (ClaimTypes.Role, user.Role)
        }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    public ActionResult ForgotUserPassword(int id)
    {
      
      return Ok();
    }
  }
}