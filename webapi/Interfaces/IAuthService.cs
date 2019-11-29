using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces {
    public interface IAuthService {
        Task<ActionResult> Login (Login data);
        Task<ActionResult> GetMeAsync (int currentUserId);
        Task<ActionResult> ChangeUserPasswordAsync (
            int currentUserId, string oldPassword, string newPassword
        );
        ActionResult ForgotUserPassword(int id);
    }
}