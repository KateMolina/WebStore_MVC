using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities.Orders;
using WebStore.Interfaces;
using WebStore.WebAPI.Clients.Base;
using WebStore_MVC.Services.Interfaces;
using WebStore_MVC.ViewModels;

namespace WebStore.WebAPI.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(HttpClient httpClient) : base(httpClient, WebAPIAddresses.Orders)
        {

        }

        public async Task<Order> CreateOrder(string userName, CartViewModel cart, OrderViewModel orderViewModel)
        {
            var create_order_model = new CreateOrderDTO
            {
                Items = cart.ToDTO(),
                Order = orderViewModel,
            };

            var response = await PostAsync($"{Address}/{userName}", create_order_model).ConfigureAwait(false);
            var order_dto = await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<OrderDTO>().ConfigureAwait(false);
            return order_dto.FromDTO();
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order_dto = await GetAsync<OrderDTO>($"{Address}/{id}").ConfigureAwait(false);
            return order_dto.FromDTO();
        }

        public async Task<IEnumerable<Order>> GetUserOrders(string userName)
        {
            var orders_dto = await GetAsync<IEnumerable<OrderDTO>>($"{Address}/user/{userName}").ConfigureAwait(false);
            return orders_dto.FromDTO();
        }
    }
}
