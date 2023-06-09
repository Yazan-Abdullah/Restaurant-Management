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
        //Get All Menu 
        public List<Menu> GetMenus()
        {
            List<Menu> menus = _dbContext.Menus.ToList();
            return menus;
        } 
        //Get Menu By Id
        public Menu GetMenuById(int id)
        {
            var menu = _dbContext.Menus.Where(x => x.MenuId == id).SingleOrDefault();
            return menu;
        }
        // Create Menu 
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
        // Update Menu
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
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //Delet Menu
        public async Task DeleteMenuAsync(int id)
        {
            try
            {
                var menu = await _dbContext.Menus.FindAsync(id);
                if (menu != null)
                {
                    _dbContext.Menus.Remove(menu);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Customer
        public List<Customer> GetCustomers()
        {
            List<Customer> customers = _dbContext.Customers.ToList();
            return customers;
        }
       
        public Customer GetCustomersById(int id)
        {
            var customer = _dbContext.Customers.FirstOrDefault(x => x.CustomerId == id);
            return customer;
        }
        public async Task<Customer> CreateCustomerAsync(AddCustomerDTO customerDto)
        {
            try
            {
                var customer = new Customer
                {
                    Name = customerDto.Name,
                    Email = customerDto.Email,
                    Password = customerDto.Password,
                    Phone = customerDto.Phone
                };

                _dbContext.Customers.Add(customer);
                await _dbContext.SaveChangesAsync();

                return customer;
            }
            catch (Exception ex)
            {               
                throw; 
            }
        }
        public async Task<Customer> UpdateCustomerAsync(int id, UpdateCustomerDTO customerDto)
        {
            try
            {
                var customer = await _dbContext.Customers.FindAsync(id);
                if (customer != null)
                {
                    customer.Name = customerDto.Name;
                    customer.Email = customerDto.Email;
                    customer.Password = customerDto.Password;
                    customer.Phone = customerDto.Phone;

                    await _dbContext.SaveChangesAsync();

                    return customer;
                }
                return null;             
            }
            catch (Exception ex)
            {             
                throw;
            }
        }
        public async Task<bool> DeleteCustomerAsync(int id)
        {
            try
            {
                var customer = await _dbContext.Customers.FindAsync(id);
                if (customer == null)
                {
                    return false;
                }

                _dbContext.Customers.Remove(customer);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw; 
            }
        }
    }
}
