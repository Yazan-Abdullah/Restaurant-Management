using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Management.Core.DTO;
using Restaurant_Management.Core.Models;
using Restaurant_Management.Core.Repository;

namespace Restaurant_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMenuRepo _MenuRepo;
        private readonly IConfiguration _configuration;
        public CustomerController(IMenuRepo menuRepo, IConfiguration configuration)
        {
            _MenuRepo = menuRepo;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("AllCustomer")]
        public IActionResult  GetCustomer(int pageSize, int pageNumber)
        {
            List<Customer> customers =  _MenuRepo.GetCustomers();
            int skipAmount = pageSize * pageNumber - (pageSize);
            return Ok(customers.Skip(skipAmount).Take(pageSize));
        }
        [HttpGet]
        [Route("GetCustomerById/{id}")]
        public IActionResult GetCustomersById(int id)
        {
            try
            {
                var customer = _MenuRepo.GetCustomersById(id);

                if (customer != null)
                {
                    return Ok(customer);

                }
                return NotFound();
            }
            catch(Exception ex)
            {
                return NotFound();
            }
        }
        [HttpPost]
        [Route("AddCustomers")]
        public async Task<IActionResult> CreateCustomer([FromBody] AddCustomerDTO customerDto)
        {
            try
            {
                var customer = await _MenuRepo.CreateCustomerAsync(customerDto);
                return CreatedAtRoute("GetCustomerById", new { id = customer.CustomerId }, customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the customer.");
            }
        }
        [HttpPut]
        [Route("UpdateCustomer/{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDTO customerDto)
        {
            try
            {
                var updatedCustomer = await _MenuRepo.UpdateCustomerAsync(id, customerDto);
                if (updatedCustomer != null)
                {
                    return Ok(updatedCustomer);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the customer.");
            }
        }
        [HttpDelete]
        [Route("DeleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var isDeleted = await _MenuRepo.DeleteCustomerAsync(id);
                if (!isDeleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the customer.");
            }
        }
    }
}
