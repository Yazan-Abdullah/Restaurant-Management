using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Restaurant_Management.Controllers;
using Restaurant_Management.Core.DTO;
using Restaurant_Management.Core.Models;
using Restaurant_Management.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Test_Restaurant_Management
{
    public class TestEmployee
    {
        private readonly Mock<IOrderRepo> _orderRepoMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly EmployeeController _employeeController;

        public TestEmployee()
        {
            _orderRepoMock = new Mock<IOrderRepo>();
            _configurationMock = new Mock<IConfiguration>();
            _employeeController = new EmployeeController(_orderRepoMock.Object, _configurationMock.Object);
        }
        [Fact]
        public void GetEmployee_ReturnsOkResult()
        {
            int pageSize = 10;
            int pageNumber = 1;

            List<Employee> employees = new List<Employee> { new Employee(), new Employee() };
            _orderRepoMock.Setup(repo => repo.GetEmployees()).Returns(employees);

            var result = _employeeController.GetEmployee(pageSize, pageNumber);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetEmployeeById_ExistingId_ReturnsOkResult()
        {
            int existingId = 1;
            var existingEmployee = new Employee { EmployeeId = existingId };

            _orderRepoMock.Setup(repo => repo.GetEmployeesById(existingId)).Returns(existingEmployee);

            var result = _employeeController.GetEmployeeById(existingId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetEmployeeById_NonExistingId_ReturnsNotFoundResult()
        {
            int nonExistingId = 999;

            _orderRepoMock.Setup(repo => repo.GetEmployeesById(nonExistingId)).Returns((Employee)null);

            var result = _employeeController.GetEmployeeById(nonExistingId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateEmployee_ReturnsOkResult()
        {
            var employeeDto = new AddEmployeeDTO();

            var createdEmployee = new Employee { EmployeeId = 1 };
            _orderRepoMock.Setup(repo => repo.CreateEmployeeAsync(employeeDto)).ReturnsAsync(createdEmployee);

            var result = await _employeeController.CreateEmployee(employeeDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateEmployee_ExistingId_ReturnsOkResult()
        {
            int existingId = 1;
            var employeeDto = new UpdateEmployeeDTO();
            var updatedEmployee = new Employee { EmployeeId = existingId };

            _orderRepoMock.Setup(repo => repo.UpdateEmployeeAsync(existingId, employeeDto)).ReturnsAsync(updatedEmployee);

            var result = await _employeeController.UpdateEmployee(existingId, employeeDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateEmployee_NonExistingId_ReturnsNotFoundResult()
        {
            int nonExistingId = 999;
            var employeeDto = new UpdateEmployeeDTO();

            _orderRepoMock.Setup(repo => repo.UpdateEmployeeAsync(nonExistingId, employeeDto)).ReturnsAsync((Employee)null);

            var result = await _employeeController.UpdateEmployee(nonExistingId, employeeDto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_ExistingId_ReturnsNoContentResult()
        {
            int existingId = 1;

            _orderRepoMock.Setup(repo => repo.DeleteEmployeeByIdAsync(existingId)).ReturnsAsync(true);

            var result = await _employeeController.DeleteEmployee(existingId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_NonExistingId_ReturnsNotFoundResult()
        {
            int nonExistingId = 999;

            _orderRepoMock.Setup(repo => repo.DeleteEmployeeByIdAsync(nonExistingId)).ReturnsAsync(false);

            var result = await _employeeController.DeleteEmployee(nonExistingId);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
    

