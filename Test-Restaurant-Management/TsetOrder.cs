using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Restaurant_Management.Controllers;
using Restaurant_Management.Core.DTO;
using Restaurant_Management.Core.Models;
using Restaurant_Management.Core.Repository;
using Restaurant_Management.Infra.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Test_Restaurant_Management
{
    public class TsetOrder
    {
        private readonly Mock<IOrderRepo> _orderRepoMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<RestaurantContext> _dbContextMock;
        private readonly OrderController _orderController;

        public TsetOrder()
        {
            _orderRepoMock = new Mock<IOrderRepo>();
            _configurationMock = new Mock<IConfiguration>();
            _dbContextMock = new Mock<RestaurantContext>();
            _orderController = new OrderController(_orderRepoMock.Object,_configurationMock.Object, _dbContextMock.Object);
        }
        [Fact]
        public void GetOrder_ValidToken_ReturnsOkResult()
        {
            string validToken = "valid_token";
            int pageSize = 10;
            int pageNumber = 1;

            SetHelperMock(true);

            List<Order> orders = new List<Order> { new Order(), new Order() };
            _orderRepoMock.Setup(repo => repo.GetOrders()).Returns(orders);

            var result = _orderController.GetOrder(validToken, pageSize, pageNumber);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetOrder_InvalidToken_ReturnsUnauthorizedResult()
        {
            string invalidToken = "invalid_token";
            int pageSize = 10;
            int pageNumber = 1;

            SetHelperMock(false);

            var result = _orderController.GetOrder(invalidToken, pageSize, pageNumber);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void GetOrderById_ExistingId_ReturnsOkResult()
        {
            int existingId = 1;
            var existingOrder = new Order { OrderId = existingId };

            _orderRepoMock.Setup(repo => repo.GetOrderById(existingId)).Returns(existingOrder);

            var result = _orderController.GetOrderById(existingId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetOrderById_NonExistingId_ReturnsNotFoundResult()
        {
            int nonExistingId = 999;

            // Mock the GetOrderById method of IOrderRepo to return null
            _orderRepoMock.Setup(repo => repo.GetOrderById(nonExistingId)).Returns((Order)null);


            var result = _orderController.GetOrderById(nonExistingId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateOrder_ValidToken_ReturnsOkResult()
        {
            string validToken = "valid_token";
            var orderDto = new AddOrderDTO();

            SetHelperMock(true);

            var createdOrder = new Order();
            _orderRepoMock.Setup(repo => repo.CreateOrderAsync(orderDto)).ReturnsAsync(createdOrder);

            var result = await _orderController.CreateOrder(validToken, orderDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateOrder_InvalidToken_ReturnsUnauthorizedResult()
        {
            string invalidToken = "invalid_token";
            var orderDto = new AddOrderDTO();

            SetHelperMock(false);

            var result = await _orderController.CreateOrder(invalidToken, orderDto);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task UpdateOrder_ValidToken_ReturnsOkResult()
        {
            int orderId = 1;
            var orderDto = new UpdateOrderDTO();
            string validToken = "valid_token";

            // Mock the helper's ValidateJWTtoken method to return true
            SetHelperMock(true);

            var result = await _orderController.UpdateOrder(orderId, orderDto, validToken);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateOrder_InvalidToken_ReturnsUnauthorizedResult()
        {
            int orderId = 1;
            var orderDto = new UpdateOrderDTO();
            string invalidToken = "invalid_token";

            SetHelperMock(false);

            var result = await _orderController.UpdateOrder(orderId, orderDto, invalidToken);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task DeleteOrder_ValidToken_ReturnsOkResult()
        {
            int orderId = 1;
            string validToken = "valid_token";

            SetHelperMock(true);

            var result = await _orderController.DeleteOrder(validToken, orderId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteOrder_InvalidToken_ReturnsUnauthorizedResult()
        {
            int orderId = 1;
            string invalidToken = "invalid_token";

            SetHelperMock(false);

            var result = await _orderController.DeleteOrder(invalidToken, orderId);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        // Helper method to set up the mock for the Helper class and its ValidateJWTtoken method
        private void SetHelperMock(bool validateResult)
        {
            var helperMock = new Mock<Helper>(null, null);
            helperMock.Setup(helper => helper.ValidateJWTtoken(It.IsAny<string>())).Returns(validateResult);
            var helperInstance = helperMock.Object;
            var field = typeof(OrderController).GetField("_helper", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(_orderController, helperInstance);
        }
    }
}
