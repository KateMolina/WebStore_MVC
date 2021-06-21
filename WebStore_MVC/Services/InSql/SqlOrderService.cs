using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Entities.Orders;
using WebStore_MVC.Services.Interfaces;
using WebStore_MVC.ViewModels;

namespace WebStore_MVC.Services.InSql
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreDB db;
        private readonly UserManager<User> userManager;

        public SqlOrderService(WebStoreDB db, UserManager<User> UserManager)
        {
            this.db = db;
            userManager = UserManager;
        }

        public async Task<Order> GetOrderById(int id) => await db.Order
            .Include(order => order.User)
            .Include(order => order.Items)
            .FirstOrDefaultAsync(order => order.Id == id);

        public async Task<IEnumerable<Order>> GetUserOrder(string userName) => await db.Order
            .Include(order => order.User)
            .Include(order => order.Items)
            .Where(order => order.User.UserName == userName)
            .ToArrayAsync();
        public async Task<Order> CreateOrder(string userName, CartViewModel cart, OrderViewModel orderViewModel)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user is null)
                throw new InvalidOperationException($"User {userName} is not in the DB");
            
            await using var transaction = await db.Database.BeginTransactionAsync();

            var order = new Order
            {
                User = user,
                Address = orderViewModel.Address,
                Phone = orderViewModel.Phone,
                Name = orderViewModel.Name,
            };

            var product_ids = cart.Items.Select(item => item.product.Id).ToArray();

            var cart_products = await db.Products
                .Where(p => product_ids.Contains(p.Id))
                .ToArrayAsync();

            order.Items = cart.Items.Join(
                cart_products,
                cart_item => cart_item.product.Id,
                cart_product => cart_product.Id,
                (cart_item, cart_product) => new OrderItem
                {
                    Order=order,
                    Product=cart_product,
                    Price=cart_product.Price,
                    Quantity=cart_item.Quantity,
                }).ToArray();

            await db.Order.AddAsync(order);
            await db.SaveChangesAsync();

            await transaction.CommitAsync();

            return order;
        }

    }
}
