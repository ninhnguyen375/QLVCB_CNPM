using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Services;

namespace webapi.Controllers
{
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomersController(IUnitOfWork unitOfWork) {
          _unitOfWork = unitOfWork;
        }

        // GET: api/customers/
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpGet]
        public ActionResult GetCustomers([FromQuery] Pagination pagination, [FromQuery] SearchCustomer search) {
          var customers = _unitOfWork.Customers.GetAll();
          
          // Search by Id:
          if (search.Id != "") {
            customers = customers.Where(c =>
              c.Id.ToLower().Contains(search.Id.ToLower()));
          }

          // Search by Phone:
          if (search.Phone != "") {
            customers = customers.Where(c =>
              c.Phone.Contains(search.Phone));
          }

          // Search by FullName:
          if (search.FullName != "") {
            customers = customers.Where(c =>
              c.FullName.ToLower().Contains(search.FullName.ToLower()));
          }

          // Sort Asc:
          if (search.sortAsc != "") {
            customers = customers.OrderBy(c =>
              c.GetType().GetProperty(search.sortAsc).GetValue(c));
          }

          // Sort Desc:
          if (search.sortDesc != "") {
            customers = customers.OrderByDescending(c =>
              c.GetType().GetProperty(search.sortDesc).GetValue(c));
          }

          return Ok (PaginatedList<Customer>.Create(customers, pagination.current, pagination.pageSize));
        }

        // GET: api/customers/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpGet ("{id}")]
        public ActionResult GetCustomer(string id) {
          var customer = _unitOfWork.Customers.Find(c =>
            c.Id.Equals(id)).SingleOrDefault();
          
          if (customer == null) {
            return NotFound (new { Id = "Mã khách hàng này không tồn tại." });
          }

          return Ok (new { success = true, data = customer });
        }

        // PUT: api/customers/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPut ("{id}")]
        public ActionResult PutCustomer(string id, EditCustomer values) {
          var customer = _unitOfWork.Customers.Find(c =>
            c.Id.Equals(id)).SingleOrDefault();
          
          if (customer == null) {
            return NotFound (new { Id = "Mã khách hàng này không tồn tại." });
          }

          if (_unitOfWork.Customers.Find(c =>
                c.Phone.Equals(values.Phone))
                .Count() != 0) {
            return BadRequest (new { Phone = "Số điện thoại này đã tồn tại." });
          }

          customer.FullName = values.FullName;
          customer.Phone = values.Phone;
          _unitOfWork.Complete();

          return Ok (new { success = true, data = customer, message = "Sửa thành công." });
        }
    }
}