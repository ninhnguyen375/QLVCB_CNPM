using System.Security.Claims;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.UseCases;
using webapi.Models.Response;

namespace webapi.Interfaces
{
    public interface IUserService
    {
        ResponseData GetUsers(Pagination pagination, SearchUser search, ClaimsPrincipal currentUser);
        ResponseData GetUser(int id);
        ResponseData PutUser(int id, SaveUserDTO saveUserDTO);
        ResponseData BlockUser(int id);
        ResponseData UnBlockUser(int id);
        ResponseData PostUser(SaveUserDTO saveUserDTO); 
        ResponseData DeleteUser(int id);
    }
}