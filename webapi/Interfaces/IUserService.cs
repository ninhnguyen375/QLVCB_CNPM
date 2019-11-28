using System.Security.Claims;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.UseCases;
using webapi.Models.Response;
using System.Threading.Tasks;

namespace webapi.Interfaces
{
    public interface IUserService
    {
        Task<ActionResult> GetUsersAsync(Pagination pagination, SearchUser search, ClaimsPrincipal currentUser);
        Task<ActionResult> GetUserAsync(int id);
        Task<ActionResult> UpdateUserAsync(int id, SaveUserDTO saveUserDTO);
        Task<ActionResult> BlockUserAsync(int id);
        Task<ActionResult> UnBlockUserAsync(int id);
        Task<ActionResult> AddUserAsync(SaveUserDTO saveUserDTO); 
        Task<ActionResult> DeleteUserAsync(int id);
    }
}