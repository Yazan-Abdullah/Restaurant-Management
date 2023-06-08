using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
