using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Management.Core.DTO;
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
        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] AddEmployeeDTO employeeDto)
        {
            try
            {
                var createdEmployee = await _orderrepo.CreateEmployeeAsync(employeeDto);
                return Ok(createdEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the employee.");
            }
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDTO employeeDto)
        {
            try
            {
                var updatedEmployee = await _orderrepo.UpdateEmployeeAsync(id, employeeDto);

                if (updatedEmployee == null)
                {
                    return NotFound();
                }

                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the employee.");
            }
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var isDeleted = await _orderrepo.DeleteEmployeeByIdAsync(id);
                if (!isDeleted)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the employee.");
            }
        }
    }
}
