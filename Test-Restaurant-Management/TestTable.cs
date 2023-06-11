using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Restaurant_Management.Controllers;
using Restaurant_Management.Core.DTO;
using Restaurant_Management.Core.Models;
using Restaurant_Management.Core.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Test_Restaurant_Management
{
    public class TestTable
    {
        private readonly Mock<IOrderRepo> _orderRepoMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly TableController _tableController;

        public TestTable()
        {
            _orderRepoMock = new Mock<IOrderRepo>();
            _configurationMock = new Mock<IConfiguration>();
            _tableController = new TableController(_orderRepoMock.Object, _configurationMock.Object);
        }
        [Fact]
        public void GetTable_ReturnsOkResult()
        {
            List<Table> tables = new List<Table> { new Table(), new Table() };
            _orderRepoMock.Setup(repo => repo.GetTables()).Returns(tables);

            var result = _tableController.GetTable();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetTableById_ExistingId_ReturnsOkResult()
        {
            int existingId = 1;
            var existingTable = new Table { TableId = existingId };
            _orderRepoMock.Setup(repo => repo.GetTableByIdAsync(existingId)).ReturnsAsync(existingTable);

            var result = await _tableController.GetTableById(existingId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetTableById_NonExistingId_ReturnsNotFoundResult()
        {
            int nonExistingId = 999;
            _orderRepoMock.Setup(repo => repo.GetTableByIdAsync(nonExistingId)).ReturnsAsync((Table)null);

            var result = await _tableController.GetTableById(nonExistingId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateTable_ReturnsOkResult()
        {
            var tableDto = new AddTableDTO();
            var createdTable = new Table { TableId = 1 };
            _orderRepoMock.Setup(repo => repo.CreateTableAsync(tableDto)).ReturnsAsync(createdTable);

            var result = await _tableController.CreateTable(tableDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateTable_ExistingId_ReturnsOkResult()
        {
            int existingId = 1;
            var tableDto = new UpdateTableDTO();
            var updatedTable = new Table { TableId = existingId };
            _orderRepoMock.Setup(repo => repo.UpdateTableAsync(existingId, tableDto)).ReturnsAsync(updatedTable);

            var result = await _tableController.UpdateTable(existingId, tableDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateTable_NonExistingId_ReturnsNotFoundResult()
        {
            int nonExistingId = 999;
            var tableDto = new UpdateTableDTO();
            _orderRepoMock.Setup(repo => repo.UpdateTableAsync(nonExistingId, tableDto)).ReturnsAsync((Table)null);

            var result = await _tableController.UpdateTable(nonExistingId, tableDto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteTable_ExistingId_ReturnsNoContentResult()
        {
            int existingId = 1;
            _orderRepoMock.Setup(repo => repo.DeleteTableAsync(existingId)).ReturnsAsync(true);

            var result = await _tableController.DeleteTable(existingId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTable_NonExistingId_ReturnsNotFoundResult()
        {
            int nonExistingId = 999;
            _orderRepoMock.Setup(repo => repo.DeleteTableAsync(nonExistingId)).ReturnsAsync(false);

            var result = await _tableController.DeleteTable(nonExistingId);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
   

