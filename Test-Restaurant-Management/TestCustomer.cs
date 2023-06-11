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
using System.Text;
using System.Threading.Tasks;

namespace Test_Restaurant_Management
{
    public class TestCustomer
    {
        private readonly Mock<IMenuRepo> _menuRepoMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly CustomerController _customerController;

        public TestCustomer()
        {
            _menuRepoMock = new Mock<IMenuRepo>();
            _configurationMock = new Mock<IConfiguration>();
            _customerController = new CustomerController(_menuRepoMock.Object, _configurationMock.Object);
        }
        [Fact]
        public void GetCustomer_ReturnsOkResult()
        {
            // Arrange
            int pageSize = 10;
            int pageNumber = 1;

            // Mock the GetCustomers method of IMenuRepo
            List<Customer> customers = new List<Customer> { new Customer(), new Customer() };
            _menuRepoMock.Setup(repo => repo.GetCustomers()).Returns(customers);

            // Act
            var result = _customerController.GetCustomer(pageSize, pageNumber);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetCustomersById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            int existingId = 1;
            var existingCustomer = new Customer { CustomerId = existingId };

            // Mock the GetCustomersById method of IMenuRepo
            _menuRepoMock.Setup(repo => repo.GetCustomersById(existingId)).Returns(existingCustomer);

            // Act
            var result = _customerController.GetCustomersById(existingId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetCustomersById_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int nonExistingId = 999;

            // Mock the GetCustomersById method of IMenuRepo to return null
            _menuRepoMock.Setup(repo => repo.GetCustomersById(nonExistingId)).Returns((Customer)null);

            // Act
            var result = _customerController.GetCustomersById(nonExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateCustomer_ReturnsCreatedAtRouteResult()
        {
            // Arrange
            var customerDto = new AddCustomerDTO();

            // Mock the CreateCustomerAsync method of IMenuRepo
            var createdCustomer = new Customer { CustomerId = 1 };
            _menuRepoMock.Setup(repo => repo.CreateCustomerAsync(customerDto)).ReturnsAsync(createdCustomer);

            // Act
            var result = await _customerController.CreateCustomer(customerDto);

            // Assert
            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async Task UpdateCustomer_ExistingId_ReturnsOkResult()
        {
            // Arrange
            int existingId = 1;
            var customerDto = new UpdateCustomerDTO();
            var updatedCustomer = new Customer { CustomerId = existingId };

            // Mock the UpdateCustomerAsync method of IMenuRepo
            _menuRepoMock.Setup(repo => repo.UpdateCustomerAsync(existingId, customerDto)).ReturnsAsync(updatedCustomer);

            // Act
            var result = await _customerController.UpdateCustomer(existingId, customerDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateCustomer_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int nonExistingId = 999;
            var customerDto = new UpdateCustomerDTO();

            // Mock the UpdateCustomerAsync method of IMenuRepo to return null
            _menuRepoMock.Setup(repo => repo.UpdateCustomerAsync(nonExistingId, customerDto)).ReturnsAsync((Customer)null);

            // Act
            var result = await _customerController.UpdateCustomer(nonExistingId, customerDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_ExistingId_ReturnsNoContentResult()
        {
            // Arrange
            int existingId = 1;

            // Mock the DeleteCustomerAsync method of IMenuRepo
            _menuRepoMock.Setup(repo => repo.DeleteCustomerAsync(existingId)).ReturnsAsync(true);

            // Act
            var result = await _customerController.DeleteCustomer(existingId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int nonExistingId = 999;

            // Mock the DeleteCustomerAsync method of IMenuRepo to return false
            _menuRepoMock.Setup(repo => repo.DeleteCustomerAsync(nonExistingId)).ReturnsAsync(false);

            // Act
            var result = await _customerController.DeleteCustomer(nonExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
    

