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
        public List<Customer> GetCustomers();
        public Customer GetCustomersById(int id);
        Task UpdateMenuAsync(int id, UpdateMenuDTO updateMenuDto);
    }
}
