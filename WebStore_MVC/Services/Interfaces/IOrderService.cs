using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Orders;
using WebStore_MVC.ViewModels;

namespace WebStore_MVC.Services.Interfaces
{
    interface IOrderService
    {
        Task<IEnumerable<Order>> GetUserOrder(string userName);

        Task<Order> GetOrderById(int id);

        Task<Order> CreateOrder(string userName, CartViewModel cart, OrderViewModel orderViewModel);
    }
}
