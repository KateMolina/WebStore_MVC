using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Domain.Entities
{
    public class Cart
    {
        public ICollection<CartItem> cartItems { get; set; } = new List<CartItem>();

        public int ItemsCount => cartItems?.Sum(item => item.Quantity) ?? 0;
    }

    public class CartItem
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

    }
}
