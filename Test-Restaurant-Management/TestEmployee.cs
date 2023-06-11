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
            // Arrange
            int pageSize = 10;
            int pageNumber = 1;

            // Mock the GetEmployees method of IOrderRepo
            List<Employee> employees = new List<Employee> { new Employee(), new Employee() };
            _orderRepoMock.Setup(repo => repo.GetEmployees()).Returns(employees);

            // Act
            var result = _employeeController.GetEmployee(pageSize, pageNumber);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetEmployeeById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            int existingId = 1;
            var existingEmployee = new Employee { EmployeeId = existingId };

            // Mock the GetEmployeesById method of IOrderRepo
            _orderRepoMock.Setup(repo => repo.GetEmployeesById(existingId)).Returns(existingEmployee);

            // Act
            var result = _employeeController.GetEmployeeById(existingId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetEmployeeById_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int nonExistingId = 999;

            // Mock the GetEmployeesById method of IOrderRepo to return null
            _orderRepoMock.Setup(repo => repo.GetEmployeesById(nonExistingId)).Returns((Employee)null);

            // Act
            var result = _employeeController.GetEmployeeById(nonExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateEmployee_ReturnsOkResult()
        {
            // Arrange
            var employeeDto = new AddEmployeeDTO();

            // Mock the CreateEmployeeAsync method of IOrderRepo
            var createdEmployee = new Employee { EmployeeId = 1 };
            _orderRepoMock.Setup(repo => repo.CreateEmployeeAsync(employeeDto)).ReturnsAsync(createdEmployee);

            // Act
            var result = await _employeeController.CreateEmployee(employeeDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateEmployee_ExistingId_ReturnsOkResult()
        {
            // Arrange
            int existingId = 1;
            var employeeDto = new UpdateEmployeeDTO();
            var updatedEmployee = new Employee { EmployeeId = existingId };

            // Mock the UpdateEmployeeAsync method of IOrderRepo
            _orderRepoMock.Setup(repo => repo.UpdateEmployeeAsync(existingId, employeeDto)).ReturnsAsync(updatedEmployee);

            // Act
            var result = await _employeeController.UpdateEmployee(existingId, employeeDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateEmployee_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int nonExistingId = 999;
            var employeeDto = new UpdateEmployeeDTO();

            // Mock the UpdateEmployeeAsync method of IOrderRepo to return null
            _orderRepoMock.Setup(repo => repo.UpdateEmployeeAsync(nonExistingId, employeeDto)).ReturnsAsync((Employee)null);

            // Act
            var result = await _employeeController.UpdateEmployee(nonExistingId, employeeDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_ExistingId_ReturnsNoContentResult()
        {
            // Arrange
            int existingId = 1;

            // Mock the DeleteEmployeeByIdAsync method of IOrderRepo
            _orderRepoMock.Setup(repo => repo.DeleteEmployeeByIdAsync(existingId)).ReturnsAsync(true);

            // Act
            var result = await _employeeController.DeleteEmployee(existingId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int nonExistingId = 999;

            // Mock the DeleteEmployeeByIdAsync method of IOrderRepo to return false
            _orderRepoMock.Setup(repo => repo.DeleteEmployeeByIdAsync(nonExistingId)).ReturnsAsync(false);

            // Act
            var result = await _employeeController.DeleteEmployee(nonExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
    

