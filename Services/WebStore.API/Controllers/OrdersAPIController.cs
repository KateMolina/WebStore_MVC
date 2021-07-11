using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Interfaces;
using WebStore_MVC.Services.Interfaces;

namespace WebStore.API.Controllers
{
    [Route(WebAPIAddresses.Orders)]
    [ApiController]
    public class OrdersAPIController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersAPIController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet("user/{UserName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDTO>))]
        public async Task<IActionResult> GetUserOrders(string UserName)
        {
            var orders = await orderService.GetUserOrders(UserName);
            return Ok(orders.ToDTO());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDTO))]
        public async Task<IActionResult> GetUserOrder(int id)
        {
            var order = await orderService.GetOrderById(id);
            return Ok(order.ToDTO());
        }

        [HttpPost("{UserName}")]
        public async Task<IActionResult> CreateOrder(string UserName,[FromBody] CreateOrderDTO orderDTO)
        {
            var order = await orderService.CreateOrder(UserName, orderDTO.Items.ToCartView(), orderDTO.Order);
            return Ok(order.ToDTO());

        }

    }
}
