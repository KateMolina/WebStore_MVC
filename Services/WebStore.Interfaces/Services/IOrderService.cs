using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Orders;
using WebStore_MVC.ViewModels;

namespace WebStore_MVC.Services.Interfaces
{
   public interface IOrderService
    {
        Task<IEnumerable<Order>> GetUserOrders(string userName);

        Task<Order> GetOrderById(int id);

        Task<Order> CreateOrder(string userName, CartViewModel cart, OrderViewModel orderViewModel);
    }
}
