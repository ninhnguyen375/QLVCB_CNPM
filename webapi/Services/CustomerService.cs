using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Interfaces;

namespace webapi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper) {
          _unitOfWork = unitOfWork;
          _mapper = mapper;
        }

        public IEnumerable<CustomerDTO> GetCustomers (Pagination pagination, SearchCustomer search) {
          // Mapping: Customer
          var customersSource = _unitOfWork.Customers.GetAll();
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

          return customers;
        }

        public CustomerDTO GetCustomer(string id) {
          // Mapping: Customer
          var customerSource = _unitOfWork.Customers.Find(c =>
            c.Id.Equals(id)).SingleOrDefault();
          var customer = _mapper.Map<Customer, CustomerDTO>(customerSource);
          
          if (customer == null) {
            return customer;
          }

          var ordersSource = _unitOfWork.Customers.GetOrdersById(customer.Id);
          _unitOfWork.Users.GetAll();
          var orders = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(ordersSource);

          customer.Orders = (ICollection<OrderDTO>) orders;

          return customer;
        }

        public DataResult PutCustomer(string id, SaveCustomerDTO saveCustomerDTO) {
          var customer = _unitOfWork.Customers.Find(c =>
            c.Id.Equals(id)).SingleOrDefault();
          
          if (customer == null) {
            return new DataResult { Error = 1 };
            
          }

          if (_unitOfWork.Customers.Find(c =>
                c.Phone.Equals(saveCustomerDTO.Phone) &&
                !c.Id.Equals(id))
                .Count() != 0) {
            return new DataResult { Error = 2 };
          }

          // Mapping: SaveCustomer
          _mapper.Map<SaveCustomerDTO, Customer>(saveCustomerDTO, customer);

          _unitOfWork.Complete();

          return new DataResult { Data = customer };
        }
    }
}