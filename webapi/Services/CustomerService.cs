using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Interfaces;

namespace webapi.Services
{
    public class CustomerService : ControllerBase, ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper) {
          _unitOfWork = unitOfWork;
          _mapper = mapper;
        }

        public async Task<ActionResult> GetCustomersAsync (Pagination pagination, SearchCustomer search) {
          // Mapping: Customer
          var customersSource = await _unitOfWork.Customers.GetAllAsync();
          var customers = _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerDTO>>(customersSource);
          
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

          return Ok (PaginatedList<CustomerDTO>.Create(customers, pagination.current, pagination.pageSize));
        }

        public async Task<ActionResult> GetCustomerAsync(string id) {
          // Mapping: Customer
          var customerSource = await _unitOfWork.Customers.FindAsync(c =>
            c.Id.Equals(id));
          var customer = _mapper.Map<Customer, CustomerDTO>(customerSource.SingleOrDefault());
          
          // Check customer exists
          if (customer == null) {
            return NotFound (new { Id = "Mã khách hàng này không tồn tại." });
          }

          // Mapping Order để lấy thông tin 
          var ordersSource = await _unitOfWork.Customers.GetOrdersByIdAsync(customer.Id);
          await _unitOfWork.Users.GetAllAsync();
          var orders = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(ordersSource);

          customer.Orders = (ICollection<OrderDTO>) orders;

          return Ok (new { success = true, data = customer });
        }

        public async Task<ActionResult> UpdateCustomerAsync(string id, SaveCustomerDTO saveCustomerDTO) {
          var customerAsync = await _unitOfWork.Customers.FindAsync(c =>
            c.Id.Equals(id));

          // Check customer exists
          var customer = customerAsync.SingleOrDefault();
          
          if (customer == null) {
            return NotFound (new { Id = "Mã khách hàng này không tồn tại." }); 
          }

          // Check phone of customer exists except self
          var customerExist = await _unitOfWork.Customers.FindAsync(c =>
            c.Phone.Equals(saveCustomerDTO.Phone) &&
            !c.Id.Equals(id));

          if (customerExist.Count() != 0) {
            return BadRequest (new { Phone = "Số điện thoại này đã tồn tại." });
          }

          // Mapping: SaveCustomer
          _mapper.Map<SaveCustomerDTO, Customer>(saveCustomerDTO, customer);

          await _unitOfWork.CompleteAsync();

          return Ok (new { success = true, data = customer, message = "Sửa thành công." });
        }
    }
}