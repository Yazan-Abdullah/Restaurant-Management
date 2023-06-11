using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Restaurant_Management.Controllers;
using Restaurant_Management.Core.DTO;
using Restaurant_Management.Core.Models;
using Restaurant_Management.Core.Repository;
using Restaurant_Management.Infra.Repository;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Test_Restaurant_Management
{
    public class MenuControllerTests
    {
        private readonly Mock<IMenuRepo> _menuRepoMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<RestaurantContext> _dbContextMock;
        private readonly MenuController _menuController;

        public MenuControllerTests()
        {
            _menuRepoMock = new Mock<IMenuRepo>();
            _configurationMock = new Mock<IConfiguration>();
            _dbContextMock = new Mock<RestaurantContext>();
            _menuController = new MenuController(_menuRepoMock.Object, _configurationMock.Object, _dbContextMock.Object);
        }

        [Fact]
        public void GetMenus_ValidToken_ReturnsOkResult()
        {
            // Arrange
            string validToken = "valid_token";
            int pageSize = 10;
            int pageNumber = 1;

            // Mock the helper's ValidateJWTtoken method to return true
            SetHelperMock(true);

            // Mock the GetMenus method of IMenuRepo
            List<Menu> menus = new List<Menu> { new Menu(), new Menu() };
            _menuRepoMock.Setup(repo => repo.GetMenus()).Returns(menus);

            // Act
            var result = _menuController.GetMenus(validToken, pageSize, pageNumber);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetMenus_InvalidToken_ReturnsUnauthorizedResult()
        {
            // Arrange
            string invalidToken = "invalid_token";
            int pageSize = 10;
            int pageNumber = 1;

            // Mock the helper's ValidateJWTtoken method to return false
            SetHelperMock(false);

            // Act
            var result = _menuController.GetMenus(invalidToken, pageSize, pageNumber);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void GetMenuById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            int existingId = 1;
            var existingMenu = new Menu { MenuId = existingId };

            // Mock the GetMenuById method of IMenuRepo
            _menuRepoMock.Setup(repo => repo.GetMenuById(existingId)).Returns(existingMenu);

            // Act
            var result = _menuController.GetMenuById(existingId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetMenuById_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int nonExistingId = 999;

            // Mock the GetMenuById method of IMenuRepo to return null
            _menuRepoMock.Setup(repo => repo.GetMenuById(nonExistingId)).Returns((Menu)null);

            // Act
            var result = _menuController.GetMenuById(nonExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task CreateMenu_ValidToken_ReturnsOkResult()
        {
            // Arrange
            string validToken = "valid_token";
            var menuDto = new AddMenuDTO();

            // Mock the helper's ValidateJWTtoken method to return true
            SetHelperMock(true);

            // Mock the CreateMenuAsync method of IMenuRepo
            _menuRepoMock.Setup(repo => repo.CreateMenuAsync(menuDto)).ReturnsAsync(new Menu());

            // Act
            var result = await _menuController.CreateMenu(validToken, menuDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateMenu_InvalidToken_ReturnsUnauthorizedResult()
        {
            // Arrange
            string invalidToken = "invalid_token";
            var menuDto = new AddMenuDTO();

            // Mock the helper's ValidateJWTtoken method to return false
            SetHelperMock(false);

            // Act
            var result = await _menuController.CreateMenu(invalidToken, menuDto);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task UpdateMenu_ValidToken_ReturnsNoContentResult()
        {
            // Arrange
            string validToken = "valid_token";
            int menuId = 1;
            var updateMenuDto = new UpdateMenuDTO();

            // Mock the helper's ValidateJWTtoken method to return true
            SetHelperMock(true);

            // Act
            var result = await _menuController.UpdateMenu(validToken, menuId, updateMenuDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateMenu_InvalidToken_ReturnsUnauthorizedResult()
        {
            // Arrange
            string invalidToken = "invalid_token";
            int menuId = 1;
            var updateMenuDto = new UpdateMenuDTO();

            // Mock the helper's ValidateJWTtoken method to return false
            SetHelperMock(false);

            // Act
            var result = await _menuController.UpdateMenu(invalidToken, menuId, updateMenuDto);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task DeleteMenu_ValidToken_ReturnsOkResult()
        {
            // Arrange
            string validToken = "valid_token";
            int menuId = 1;

            // Mock the helper's ValidateJWTtoken method to return true
            SetHelperMock(true);

            // Act
            var result = await _menuController.DeleteMenu(validToken, menuId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteMenu_InvalidToken_ReturnsUnauthorizedResult()
        {
            // Arrange
            string invalidToken = "invalid_token";
            int menuId = 1;

            // Mock the helper's ValidateJWTtoken method to return false
            SetHelperMock(false);

            // Act
            var result = await _menuController.DeleteMenu(invalidToken, menuId);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
        // Helper method to set up the mock for the Helper class and its ValidateJWTtoken method
        private void SetHelperMock(bool validateResult)
        {
            var helperMock = new Mock<Helper>(_dbContextMock.Object, _configurationMock.Object);
            helperMock.Setup(helper => helper.ValidateJWTtoken(It.IsAny<string>())).Returns(validateResult);
            var helperInstance = helperMock.Object;
            var field = typeof(MenuController).GetField("_helper", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(_menuController, helperInstance);
        }
    }
}