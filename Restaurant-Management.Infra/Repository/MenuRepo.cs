using Microsoft.AspNetCore.Http;
using Restaurant_Management.Core.DTO;
using Restaurant_Management.Core.Models;
using Restaurant_Management.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Infra.Repository
{
    public class MenuRepo : IMenuRepo
    {
        private readonly RestaurantContext _dbContext;

        public MenuRepo(RestaurantContext dbContext)
        {
            _dbContext = dbContext;
        }
        // Menu
        public List<Menu> GetMenus()
        {
            List<Menu> menus = _dbContext.Menus.ToList();
            return menus;
        } 
        public Menu GetMenuById(int id)
        {
            var menu = _dbContext.Menus.Where(x => x.MenuId == id).SingleOrDefault();
            return menu;
        }

        //Customer
        public List<Customer> GetCustomers()
        {
            List<Customer> customers = _dbContext.Customers.ToList();
            return customers;
        }
        public async Task<Menu> CreateMenuAsync(AddMenuDTO menuDto)
        {
            try
            {
                var menu = new Menu
                {
                    Name = menuDto.Name,
                    Description = menuDto.Description,
                    Price = menuDto.Price
                };

                _dbContext.Menus.Add(menu);
                await _dbContext.SaveChangesAsync();
                return menu;
            }
            catch (Exception ex)
            {              
                throw; 
            }
        }

        public Customer GetCustomersById(int id)
        {
            var customer = _dbContext.Customers.FirstOrDefault(x => x.CustomerId == id);
            return customer;
        }
        public async Task UpdateMenuAsync(int id, UpdateMenuDTO updateMenuDto)
        {
            try
            {
                var menu = await _dbContext.Menus.FindAsync(id);
                if (menu != null)
                {
                    menu.Name = updateMenuDto.Name;
                    menu.Description = updateMenuDto.Description;
                    menu.Price = updateMenuDto.Price;

                    await _dbContext.SaveChangesAsync();
                }
            }catch(Exception ex)
            {
                throw;
            }
           

            
            
        }
    }
}
