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
  public class SendMailService : ControllerBase
  {
    private Email _email;
    public SendMailService(Email email) {
      _email = email;
    }

    public static void SendMail(Email email)
    {
      SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
      MailMessage mail = new MailMessage();
      mail.From = new MailAddress("ninhnguyen.annn@gmail.com");
      mail.To.Add(email.ToEmail);
      mail.Subject = email.Subject;
      mail.Body = email.Message;
      SmtpServer.Port = 25;
      SmtpServer.Credentials = new System.Net.NetworkCredential("ninhnguyen.annn@gmail.com", "Ninhnguyen375.");
      SmtpServer.EnableSsl = true;
      SmtpServer.Send(mail);
    }
  }
}