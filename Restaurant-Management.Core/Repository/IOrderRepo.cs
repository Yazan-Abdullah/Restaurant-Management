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
        public List<Order> GetOrders();
        Task<Order> CreateOrderAsync(AddOrderDTO orderDto);
        public List<Table> GetTables();
     
        public List<Employee> GetEmployees();
        public Order GetEmployeesById(int id);
        public Employee GetTablesById(int id);
        public Table GetOrdersById(int id);
       
    }
}
