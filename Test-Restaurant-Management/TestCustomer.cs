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
            
            int pageSize = 10;
            int pageNumber = 1;

            List<Customer> customers = new List<Customer> { new Customer(), new Customer() };
            _menuRepoMock.Setup(repo => repo.GetCustomers()).Returns(customers);

            var result = _customerController.GetCustomer(pageSize, pageNumber);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetCustomersById_ExistingId_ReturnsOkResult()
        {
            int existingId = 1;
            var existingCustomer = new Customer { CustomerId = existingId };

            _menuRepoMock.Setup(repo => repo.GetCustomersById(existingId)).Returns(existingCustomer);

            var result = _customerController.GetCustomersById(existingId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetCustomersById_NonExistingId_ReturnsNotFoundResult()
        {
            int nonExistingId = 999;

            _menuRepoMock.Setup(repo => repo.GetCustomersById(nonExistingId)).Returns((Customer)null);

            var result = _customerController.GetCustomersById(nonExistingId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateCustomer_ReturnsCreatedAtRouteResult()
        {
            var customerDto = new AddCustomerDTO();

            var createdCustomer = new Customer { CustomerId = 1 };
            _menuRepoMock.Setup(repo => repo.CreateCustomerAsync(customerDto)).ReturnsAsync(createdCustomer);

            var result = await _customerController.CreateCustomer(customerDto);

            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async Task UpdateCustomer_ExistingId_ReturnsOkResult()
        {
            int existingId = 1;
            var customerDto = new UpdateCustomerDTO();
            var updatedCustomer = new Customer { CustomerId = existingId };

            _menuRepoMock.Setup(repo => repo.UpdateCustomerAsync(existingId, customerDto)).ReturnsAsync(updatedCustomer);

            var result = await _customerController.UpdateCustomer(existingId, customerDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateCustomer_NonExistingId_ReturnsNotFoundResult()
        {
            int nonExistingId = 999;
            var customerDto = new UpdateCustomerDTO();

            _menuRepoMock.Setup(repo => repo.UpdateCustomerAsync(nonExistingId, customerDto)).ReturnsAsync((Customer)null);

            var result = await _customerController.UpdateCustomer(nonExistingId, customerDto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_ExistingId_ReturnsNoContentResult()
        {
            int existingId = 1;

            _menuRepoMock.Setup(repo => repo.DeleteCustomerAsync(existingId)).ReturnsAsync(true);

            var result = await _customerController.DeleteCustomer(existingId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_NonExistingId_ReturnsNotFoundResult()
        {
            int nonExistingId = 999;

            _menuRepoMock.Setup(repo => repo.DeleteCustomerAsync(nonExistingId)).ReturnsAsync(false);

            var result = await _customerController.DeleteCustomer(nonExistingId);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
    

