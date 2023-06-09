using Restaurant_Management.Core.DTO;
using Restaurant_Management.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Core.Repository
{
    public interface IMenuRepo
    {
        public List<Menu> GetMenus();

        public Menu GetMenuById(int id);
        Task<Menu> CreateMenuAsync(AddMenuDTO menu);
        Task UpdateMenuAsync(int id, UpdateMenuDTO updateMenuDto);
        Task DeleteMenuAsync(int id);
        public List<Customer> GetCustomers();
        public Customer GetCustomersById(int id);
        Task<Customer> CreateCustomerAsync(AddCustomerDTO customerDto);
        Task<Customer> UpdateCustomerAsync(int id, UpdateCustomerDTO customerDto);
        Task<bool> DeleteCustomerAsync(int id);
    }
}
