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
        Employee IOrderRepo.GetTablesById(int id)
        {
            var employee = _dbContext.Employees.FirstOrDefault(x => x.EmployeeId == id);
            return employee;
        }

        //Order
        public List<Order> GetOrders()
        {
            List<Order> orders = _dbContext.Orders.ToList();
            return orders;
        }
        Order IOrderRepo.GetEmployeesById(int id)
        {
            var order = _dbContext.Orders.FirstOrDefault(x => x.OrderId == id);
            return order;
        }
        //Table
        public List<Table> GetTables()
        {
            List<Table> tables = _dbContext.Tables.ToList();
            return tables;
        }
        Table IOrderRepo.GetOrdersById(int id)
        {
            var table = _dbContext.Tables.FirstOrDefault(x => x.TableId == id);
            return table;
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

    }
}
