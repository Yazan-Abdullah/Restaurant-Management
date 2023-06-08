﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Management.Core.DTO;
using Restaurant_Management.Core.Models;
using Restaurant_Management.Core.Repository;
using Restaurant_Management.Infra.Repository;

namespace Restaurant_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepo _orderrepo;
        private readonly IConfiguration _configuration;
        public OrderController(IOrderRepo OrderRepo, IConfiguration configuration)
        {
            _orderrepo = OrderRepo;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("AllOrder")]
        public IActionResult GetOrder(int pageSize, int pageNumber)
        {
            List<Order> orders = _orderrepo.GetOrders();
            int skipAmount = pageSize * pageNumber - (pageSize);
            return Ok(orders.Skip(skipAmount).Take(pageSize));
        }
        [HttpGet]
        [Route("GetOrderById/{id}")]
        public IActionResult GetOrderById(int id)
        {
            try
            {
                var order = _orderrepo.GetOrdersById(id);
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
        public async Task<IActionResult> CreateOrder(AddOrderDTO orderDto)
        {
            try
            {
                Order createdOrder = await _orderrepo.CreateOrderAsync(orderDto);
                return Ok(createdOrder);
            }
            catch (Exception ex)
            {               
                return Unauthorized(ex);
            }
        }
    }
}