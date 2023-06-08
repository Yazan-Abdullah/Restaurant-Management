using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        [Route("GettableById/{id}")]
        public IActionResult GetTableById(int id)
        {
            try
            {
                var table = _orderrepo.GetTablesById(id);
                if (table != null)
                {
                    return Ok(table);
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
