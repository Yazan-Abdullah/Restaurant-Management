using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management.Core.DTO;

using Restaurant_Management.Core.Models;
using Restaurant_Management.Core.Repository;
using Restaurant_Management.Infra.Repository;
using Serilog;

namespace Restaurant_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepo _orderrepo;
        private readonly IConfiguration _configuration;
        private readonly Helper _helper;
        private readonly RestaurantContext _DbContext;
        public OrderController(IOrderRepo OrderRepo, IConfiguration configuration, RestaurantContext DbContext)
        {
            _orderrepo = OrderRepo;
            _configuration = configuration;
            _helper = new Helper(_DbContext, _configuration);
        }
        [HttpGet]
        [Route("AllOrder")]
        public IActionResult GetOrder([FromBody] string token, int pageSize, int pageNumber)
        {
            if (_helper.ValidateJWTtoken(token))
            {
                List<Order> orders = _orderrepo.GetOrders();
                int skipAmount = pageSize * pageNumber - (pageSize);
                Log.Information("Successfully Get All Order ");
                return Ok(orders.Skip(skipAmount).Take(pageSize));
            }
            Log.Error("Failed Get All Order : Invalid Token");
            return Unauthorized("Invalid Token");
        }
        [HttpGet]
        [Route("GetOrderById/{id}")]
        public IActionResult GetOrderById(int id)
        {
            try
            {
                var order = _orderrepo.GetOrderById(id);
                if (order != null)
                {
                    return Ok(order);
                }
                return NotFound();
            }
            catch(Exception ex)
            {
                return NotFound();
            }           
        }
        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder(string token, AddOrderDTO orderDto)
        {
            try
            {
                if (_helper.ValidateJWTtoken(token))
                {
                    Order createdOrder = await _orderrepo.CreateOrderAsync(orderDto);
                    Log.Information("Successfully Create Order");
                    return Ok(createdOrder);
                }
                Log.Error("Failed Create Order : Invalid Token");
                return Unauthorized("Invalid Token");
            }
            catch (Exception ex)
            {               
                return Unauthorized(ex);
            }
        }
        [HttpPut("UpdateOrder/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO orderDto, string token)
        {
            try
            {
                if (_helper.ValidateJWTtoken(token))
                {
                    await _orderrepo.UpdateOrderAsync(id, orderDto);
                    Log.Information("Successfully Update Order ");
                    return Ok("Order Updated Sucessfuly");
                }
                Log.Error("Failed Update Order : Invalid Token");
                return Unauthorized("Invalid Token");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the order.");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromBody] string token,int id)
        {
            try
            {
                if (_helper.ValidateJWTtoken(token))
                {
                    await _orderrepo.DeleteOrderAsync(id);
                    Log.Information(" Order Successfully  Deleted");
                    return Ok("Order Deleted Sucessfuly");
                }
                Log.Error("Failed Delete Order : Invalid Token");
                return Unauthorized("Invalid Token");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the order.");
            }
        }
    }
}
