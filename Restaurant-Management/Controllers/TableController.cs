using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant_Management.Core.DTO;
using Restaurant_Management.Core.Models;
using Restaurant_Management.Core.Repository;
using Restaurant_Management.Infra.Repository;
using Table = Restaurant_Management.Core.Models.Table;

namespace Restaurant_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly IOrderRepo _orderrepo;
        private readonly IConfiguration _configuration;
        public TableController(IOrderRepo OrderRepo, IConfiguration configuration)
        {
            _orderrepo = OrderRepo;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("AllTable")]
        public IActionResult GetTable()
        {
            List<Table> orders = _orderrepo.GetTables();
            return Ok(orders);
        }
        [HttpGet]
        [Route("GetTableById/{id}")]
        public async Task<IActionResult> GetTableById(int id)
        {
            try
            {
                var table = await _orderrepo.GetTableByIdAsync(id);
                if (table == null)
                {
                    return NotFound();
                }
                return Ok(table);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the table.");
            }
        }
        [HttpPost]
        [Route("CreateTable")]
        public async Task<IActionResult> CreateTable([FromBody] AddTableDTO tableDto)
        {
            try
            {
                var createdTable = await _orderrepo.CreateTableAsync(tableDto);

                return Ok(createdTable);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the table.");
            }
        }
        [HttpPut]
        [Route("UpdateTable/{id}")]
        public async Task<IActionResult> UpdateTable(int id, [FromBody] UpdateTableDTO tableDto)
        {
            try
            {
                var updatedTable = await _orderrepo.UpdateTableAsync(id, tableDto);

                if (updatedTable == null)
                {
                    return NotFound(); 
                }

                return Ok(updatedTable);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the table.");
            }
        }
        [HttpDelete]
        [Route("DeleteTable/{id}")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            try
            {
                var isDeleted = await _orderrepo.DeleteTableAsync(id);
                if (!isDeleted)
                {
                    return NotFound(); 
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the table.");
            }
        }
    }
}
