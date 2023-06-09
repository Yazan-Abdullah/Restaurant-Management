using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant_Management.Core.DTO;
using Restaurant_Management.Core.Models;
using Restaurant_Management.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Table = Restaurant_Management.Core.Models.Table;

namespace Restaurant_Management.Infra.Repository
{
    public class OrderRepo : IOrderRepo
    {
        private readonly RestaurantContext _dbContext;

        public OrderRepo(RestaurantContext dbContext)
        {
            _dbContext = dbContext;
        }
       // Employee
        public List<Employee> GetEmployees()
        {
            List<Employee> employees = _dbContext.Employees.ToList();
            return employees;
        }
        Employee IOrderRepo.GetEmployeesById(int id)
        {
            var employee = _dbContext.Employees.FirstOrDefault(x => x.EmployeeId == id);
            return employee;
        }
        public async Task<Employee> CreateEmployeeAsync(AddEmployeeDTO employeeDto)
        {
            try
            {
                var employee = new Employee
                {
                    Name = employeeDto.Name,
                    Position = employeeDto.Position
                };

                _dbContext.Employees.Add(employee);
                await _dbContext.SaveChangesAsync();

                return employee;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Employee> UpdateEmployeeAsync(int id, UpdateEmployeeDTO employeeDto)
        {
            try
            {
                var existingEmployee = await _dbContext.Employees.FindAsync(id);

                if (existingEmployee == null)
                {
                    return null; 
                }
                existingEmployee.Name = employeeDto.Name;
                existingEmployee.Position = employeeDto.Position;
                await _dbContext.SaveChangesAsync();
                return existingEmployee;
            }
            catch (Exception ex)
            {
                throw; 
            }
        }
        public async Task<bool> DeleteEmployeeByIdAsync(int id)
        {
            try
            {
                var employee = await _dbContext.Employees.FindAsync(id);

                if (employee == null)
                {
                    return false;
                }
                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //Order
        public List<Order> GetOrders()
        {
            List<Order> orders = _dbContext.Orders.ToList();
            return orders;
        }
        Order IOrderRepo.GetOrderById(int id)
        {
            var order = _dbContext.Orders.FirstOrDefault(x => x.OrderId == id);
            return order;
        }

        public async Task<Order> CreateOrderAsync(AddOrderDTO orderDto)
        {
            try
            {

                var orderItems = orderDto.OrderItems.Select(item => new OrderItem
                {
                    MenuId = item.MenuId,
                    Quantity = item.Quantity
                }).ToList();

                var order = new Order
                {
                    TableId = orderDto.TableId,
                    CustomerId = orderDto.CustomerId,
                    OrderItems = orderItems,
                    TotalPrice = orderDto.TotalPrice
                };

                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();

                return order;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<Order> UpdateOrderAsync(int id, UpdateOrderDTO orderDto)
        {
            try
            {
                var order = await _dbContext.Orders.Include(x => x.OrderItems).FirstOrDefaultAsync(y => y.OrderId == id);

                if (order != null)
                {                   
                    order.CustomerId = orderDto.CustomerId;
                    order.TableId = orderDto.TableId;
                    order.TotalPrice = orderDto.TotalPrice;
                    foreach (var itemDto in orderDto.OrderItems)
                    {
                        var orderItem = order.OrderItems.FirstOrDefault(oi => oi.OrderItemId == itemDto.OrderItemId);

                        if (orderItem != null)
                        {
                            orderItem.MenuId = itemDto.MenuId;
                            orderItem.Quantity = itemDto.Quantity;
                        }
                    }                   
                }                        
                await _dbContext.SaveChangesAsync();              
                return order;
            }
            catch (Exception ex)
            {                
                throw;
            }
        }
        public async Task DeleteOrderAsync(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);

            if (order != null)
            {
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Order not found.");
            }
        }
        //Table
        public List<Table> GetTables()
        {
            List<Table> tables = _dbContext.Tables.ToList();
            return tables;
        }
        public async Task<Table> GetTableByIdAsync(int id)
        {
            try
            {
                var table = await _dbContext.Tables.FindAsync(id);
                return table;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
