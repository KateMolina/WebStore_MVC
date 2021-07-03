using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Orders;
using WebStore_MVC.ViewModels;

namespace WebStore.Domain.DTO
{
    public class OrderDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }

        public IEnumerable<OrderItemDTO> Items { get; set; }

    }

    public class OrderItemDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }

    public class CreateOrderDTO
    {
        public OrderViewModel Order { get; set; }

        public IEnumerable<OrderItemDTO> Items { get; set; }
    }

    public static class OrderItemMapper
    {
        public static OrderItemDTO ToDTO(this OrderItem item) => item is null
            ? null
            : new OrderItemDTO
            {
                Id = item.Id,
                ProductId = item.Product.Id,
                Price = item.Price,
                Quantity = item.Quantity,
            };

        public static OrderItem FromDTO(this OrderItemDTO item) => item is null
            ? null
            : new OrderItem
            {
                Id = item.Id,
                Product = new Product { Id = item.Id },
                Price = item.Price,
                Quantity = item.Quantity,
            };

    }
    public static class OrderMapper
    {
        public static OrderDTO ToDTO(this Order order) => order is null
            ? null
            : new OrderDTO
            {
                Id = order.Id,
                Name = order.Name,
                Phone = order.Phone,
                Address = order.Address,
                Date = order.Date,
                Items = order.Items.Select(OrderItemMapper.ToDTO),
            };

        public static Order FromDTO(this OrderDTO order) => order is null
            ? null
            : new Order
            {
                Id = order.Id,
                Name = order.Name,
                Phone = order.Phone,
                Address = order.Address,
                Date = order.Date,
                Items = order.Items.Select(OrderItemMapper.FromDTO).ToList(),
            };


        public static IEnumerable<OrderDTO> ToDTO(this IEnumerable<Order> order) => order.Select(ToDTO);
        public static IEnumerable<Order> FromDTO(this IEnumerable<OrderDTO> order) => order.Select(FromDTO);

        public static IEnumerable<OrderItemDTO> ToDTO(this CartViewModel cart) => cart.Items.Select(p => new OrderItemDTO
        {
            ProductId = p.product.Id,
            Price = p.product.Price,
            Quantity = p.Quantity,
        });

        public static CartViewModel ToCartView(this IEnumerable<OrderItemDTO> items) =>
            new()
            {
                Items = items.Select(p => (new ProductViewModel { Id = p.ProductId }, p.Quantity))
            };

    }

}
