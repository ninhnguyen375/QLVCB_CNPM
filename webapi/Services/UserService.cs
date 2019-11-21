using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

    public ResponseData GetUsers (Pagination pagination, SearchUser search, ClaimsPrincipal currentUser) {
      var usersQuery = _unitOfWork.Users;
      var users = usersQuery.GetAll ();
      // Searching
      if (search.email != "") {
        users = users.Where (u => u.Email.Contains (search.email));
      }
      if (search.fullname != "") {
        users = users.Where (u => u.FullName.Contains (search.fullname));
      }
      if (search.identifier != "") {
        users = users.Where (u => u.Identifier.Equals (search.identifier));
      }
      if (search.phone != "") {
        users = users.Where (u => u.Phone.Equals (search.phone));
      }
      // Sorting
      if (search.sortAsc != "") {
        Console.WriteLine (search.sortAsc);
        users = users.OrderBy (u => u.GetType ().GetProperty (search.sortAsc).GetValue (u, null));
      }
      if (search.sortDesc != "") {
        Console.WriteLine (search.sortDesc);
        users = users.OrderByDescending (u => u.GetType ().GetProperty (search.sortDesc).GetValue (u, null));
      }

      if (currentUser.IsInRole ("ADMIN"))
        users = users.Where (i => i.Role.Equals ("STAFF"));
      else
        return new ResponseData { Forbid = "Quyền truy cập bị từ chối." };

      var mapped = _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>> (users);
      var paginatedList = PaginatedList<UserDTO>.Create (mapped, pagination.current, pagination.pageSize);

      return new ResponseData { Data = paginatedList };
    }

    public ResponseData GetUser (int id) {
      var user = _unitOfWork.Users.GetBy (id);

      if (user == null) {
        return new ResponseData { NotFound = "Nhân viên không tồn tại." };
      }

      var data = _mapper.Map<User, UserDTO> (user);
      return new ResponseData { Data = new { data } };
    }

    public ResponseData PutUser (int id, SaveUserDTO saveUserDTO) {
      var user = _unitOfWork.Users.GetBy (id);

      // Check exists
      if (_unitOfWork.Users.Find (
          u => u.Identifier.Equals (saveUserDTO.Identifier) &&
          u.Id != id
        ).Count () != 0) {
        return (new ResponseData {
          BadRequest = new {
            Identifier = "CMND đã được sử dụng"
          }
        });
      }
      if (_unitOfWork.Users.Find (
          u => u.Email.Equals (saveUserDTO.Email) &&
          u.Id != id
        ).Count () != 0) {
        return (new ResponseData {
          BadRequest = new {
            Email = "Email đã được sử dụng"
          }
        });
      }

      // Mapping: SaveUser
      _mapper.Map<SaveUserDTO, User> (saveUserDTO, user);

      _unitOfWork.Complete ();

      return new ResponseData { };
    }

    public ResponseData BlockUser (int id) {
      var user = _unitOfWork.Users.GetBy (id);

      if (user == null) {
        return new ResponseData { NotFound = "Nhân viên không tồn tại." };
      }

      user.Status = 2; // 2: Banned
      _unitOfWork.Complete ();

      return new ResponseData { };
    }

    public ResponseData UnBlockUser (int id) {
      var user = _unitOfWork.Users.GetBy (id);

      if (user == null) {
        return new ResponseData { NotFound = "Nhân viên không tồn tại." };
      }

      user.Status = 1; // 1: Active
      _unitOfWork.Complete ();

      return new ResponseData { };
    }

    public ResponseData PostUser (SaveUserDTO saveUserDTO) {
      if (_unitOfWork.Users.Find (u => u.Identifier.Equals (saveUserDTO.Identifier)).Count () != 0) {
        return (new ResponseData {
          BadRequest = (new {
            Identifier = "CMND đã được sử dụng"
          })
        });
      }
      if (_unitOfWork.Users.Find (u => u.Email.Equals (saveUserDTO.Email)).Count () != 0) {
        return (new ResponseData {
          BadRequest = (new {
            Email = "Email đã được sử dụng"
          })
        });
      }

      string defaultPassword = "12345678";

      saveUserDTO.Password = BCrypt.Net.BCrypt.HashPassword (defaultPassword);

      var user = _mapper.Map<SaveUserDTO, User> (saveUserDTO);
      _unitOfWork.Users.Add (user);
      _unitOfWork.Complete ();

      return new ResponseData { };
    }

    public ResponseData DeleteUser (int id) {
      var user = _unitOfWork.Users.GetBy (id);

      if (user == null) {
        return new ResponseData { NotFound = "Nhân viên không tồn tại." };
      }

      _unitOfWork.Users.Remove (user);
      _unitOfWork.Complete ();

      return new ResponseData { };
    }
  }
}