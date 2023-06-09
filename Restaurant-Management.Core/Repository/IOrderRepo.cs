using Restaurant_Management.Core.DTO;
using Restaurant_Management.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Core.Repository
{
    public interface IOrderRepo
    { 
        // Order
        public List<Order> GetOrders();
        public Order GetOrderById(int id);
        Task<Order> CreateOrderAsync(AddOrderDTO orderDto);
        public Task<Order> UpdateOrderAsync(int id, UpdateOrderDTO orderDto);
        Task DeleteOrderAsync(int id);
        // Table
        public List<Table> GetTables();
        Task<Table> GetTableByIdAsync(int id);

        // Employee
        public Employee GetEmployeesById(int id);
        public List<Employee> GetEmployees();
        Task<Employee> CreateEmployeeAsync(AddEmployeeDTO employeeDto);
        public Task<Employee> UpdateEmployeeAsync(int id, UpdateEmployeeDTO employeeDto);
        Task<bool> DeleteEmployeeByIdAsync(int id);
    }
}
