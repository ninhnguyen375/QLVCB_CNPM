using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Interfaces;
using webapi.Models.Response;

namespace webapi.Services {
  public class UserService : IUserService {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService (IUnitOfWork unitOfWork, IMapper mapper) {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }

    public async Task<ResponseData> GetUsersAsync (Pagination pagination, SearchUser search, ClaimsPrincipal currentUser) {
      var usersQuery = _unitOfWork.Users;
      var users = await usersQuery.GetAllAsync ();
      
      var mapped = _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>> (users);

      // Searching
      if (search.email != "") {
        mapped = mapped.Where (u => u.Email.Contains (search.email));
      }
      if (search.fullname != "") {
        mapped = mapped.Where (u => u.FullName.Contains (search.fullname));
      }
      if (search.identifier != "") {
        mapped = mapped.Where (u => u.Identifier.Equals (search.identifier));
      }
      if (search.phone != "") {
        mapped = mapped.Where (u => u.Phone.Equals (search.phone));
      }
      // Sorting
      if (search.sortAsc != "") {
        Console.WriteLine (search.sortAsc);
        mapped = mapped.OrderBy (u => u.GetType ().GetProperty (search.sortAsc).GetValue (u, null));
      }
      if (search.sortDesc != "") {
        Console.WriteLine (search.sortDesc);
        mapped = mapped.OrderByDescending (u => u.GetType ().GetProperty (search.sortDesc).GetValue (u, null));
      }

      if (currentUser.IsInRole ("ADMIN"))
        mapped = mapped.Where (i => i.Role.Equals ("STAFF"));
      else
        return new ResponseData { Forbid = "Quyền truy cập bị từ chối." };

      // var mapped = _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>> (users);
      var paginatedList = PaginatedList<UserDTO>.Create (mapped, pagination.current, pagination.pageSize);

      return new ResponseData { Data = paginatedList };
    }

    public async Task<ResponseData> GetUserAsync (int id) {
      var user = await _unitOfWork.Users.GetByAsync (id);

      if (user == null) {
        return new ResponseData { NotFound = "Nhân viên không tồn tại." };
      }

      var data = _mapper.Map<User, UserDTO> (user);
      return new ResponseData { Data = new { data } };
    }

    public async Task<ResponseData> PutUserAsync (int id, SaveUserDTO saveUserDTO) {
      var user = await _unitOfWork.Users.GetByAsync (id);

      // Check exists
      var userAsync = await _unitOfWork.Users.FindAsync (
        u => u.Identifier.Equals (saveUserDTO.Identifier) &&
        u.Id != id);

      if (userAsync.Count () != 0) {
        return (new ResponseData {
          BadRequest = new {
            Identifier = "CMND đã được sử dụng"
          }
        });
      }

      userAsync = await _unitOfWork.Users.FindAsync (
          u => u.Email.Equals (saveUserDTO.Email) &&
          u.Id != id);

      if (userAsync.Count () != 0) {
        return (new ResponseData {
          BadRequest = new {
            Email = "Email đã được sử dụng"
          }
        });
      }

      // Mapping: SaveUser
      _mapper.Map<SaveUserDTO, User> (saveUserDTO, user);

      await _unitOfWork.CompleteAsync ();

      return new ResponseData { };
    }

    public async Task<ResponseData> BlockUserAsync (int id) {
      var user = await _unitOfWork.Users.GetByAsync (id);

      if (user == null) {
        return new ResponseData { NotFound = "Nhân viên không tồn tại." };
      }

      user.Status = 2; // 2: Banned
      await _unitOfWork.CompleteAsync ();

      return new ResponseData { };
    }

    public async Task<ResponseData> UnBlockUserAsync (int id) {
      var user = await _unitOfWork.Users.GetByAsync (id);

      if (user == null) {
        return new ResponseData { NotFound = "Nhân viên không tồn tại." };
      }

      user.Status = 1; // 1: Active
      await _unitOfWork.CompleteAsync ();

      return new ResponseData { };
    }

    public async Task<ResponseData> PostUserAsync (SaveUserDTO saveUserDTO) {
      var userAsync = await _unitOfWork.Users.FindAsync (u => 
        u.Identifier.Equals (saveUserDTO.Identifier));

      if (userAsync.Count () != 0) {
        return (new ResponseData {
          BadRequest = (new {
            Identifier = "CMND đã được sử dụng"
          })
        });
      }

      userAsync = await _unitOfWork.Users.FindAsync (u => 
        u.Email.Equals (saveUserDTO.Email));

      if (userAsync.Count () != 0) {
        return (new ResponseData {
          BadRequest = (new {
            Email = "Email đã được sử dụng"
          })
        });
      }

      string defaultPassword = "12345678";

      saveUserDTO.Password = BCrypt.Net.BCrypt.HashPassword (defaultPassword);

      var user = _mapper.Map<SaveUserDTO, User> (saveUserDTO);
      await _unitOfWork.Users.AddAsync (user);
      await _unitOfWork.CompleteAsync ();

      return new ResponseData { };
    }

    public async Task<ResponseData> DeleteUserAsync (int id) {
      var user = await _unitOfWork.Users.GetByAsync (id);

      if (user == null) {
        return new ResponseData { NotFound = "Nhân viên không tồn tại." };
      }

      await _unitOfWork.Users.RemoveAsync (user);
      await _unitOfWork.CompleteAsync ();

      return new ResponseData { };
    }
  }
}