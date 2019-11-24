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
        Task<ResponseData> GetUsersAsync(Pagination pagination, SearchUser search, ClaimsPrincipal currentUser);
        Task<ResponseData> GetUserAsync(int id);
        Task<ResponseData> PutUserAsync(int id, SaveUserDTO saveUserDTO);
        Task<ResponseData> BlockUserAsync(int id);
        Task<ResponseData> UnBlockUserAsync(int id);
        Task<ResponseData> PostUserAsync(SaveUserDTO saveUserDTO); 
        Task<ResponseData> DeleteUserAsync(int id);
    }
}