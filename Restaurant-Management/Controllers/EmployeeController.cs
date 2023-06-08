using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Management.Core.Models;
using Restaurant_Management.Core.Repository;
using Restaurant_Management.Infra.Repository;
using System;

namespace Restaurant_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IOrderRepo _orderrepo;
        private readonly IConfiguration _configuration;
        public EmployeeController(IOrderRepo OrderRepo, IConfiguration configuration)
        {
            _orderrepo = OrderRepo;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("AllEmployee")]
        public IActionResult GetEmployee(int pageSize, int pageNumber)
        {
            List<Employee> employees = _orderrepo.GetEmployees();
            int skipAmount = pageSize * pageNumber - (pageSize);
            return Ok(employees.Skip(skipAmount).Take(pageSize));
        }
        [HttpGet]
        [Route("GetEmployeeById/{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            try
            {
                var employee = _orderrepo.GetEmployeesById(id);
                if (employee != null)
                {
                    return Ok(employee);

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
