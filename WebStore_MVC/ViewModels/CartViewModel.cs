using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore_MVC.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<(ProductViewModel product, int Quantity)> Items { get; set; }

        public int ItemCount => Items?.Sum(item => item.Quantity) ?? 0;

        public decimal TotalSum => Items?.Sum(i => i.product.Price * i.Quantity) ?? 0m;
    }
}
