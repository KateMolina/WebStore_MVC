using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assert = Xunit.Assert;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebStore.Domain.Entities;
using WebStore_MVC.ViewModels;

namespace WebStore.Services.Tests.Services
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _Cart;

        [TestInitialize]
        public void Initialize()
        {
            _Cart = new Cart
            {
                CartItems = new List<CartItem>()
                {
                    new (){ProductId = 1, Quantity = 1},
                    new (){ProductId = 2, Quantity =3}
                }
            };

        }

        [TestMethod]
        public void Cart_Class_ItemCount_Returns_correct_Quantity()
        {
            var cart = _Cart;
            var expected_items_count = _Cart.CartItems.Sum(i => i.Quantity);
            var actual_items_count = cart.ItemsCount;

            Assert.Equal(expected_items_count, actual_items_count);

        }



        [TestMethod]
        public void CartViewModel_Returns_correct_Quantity()
        {
            var cart_view_model = new CartViewModel
            {
                Items = new[]
                {
                    (new ProductViewModel{Id = 1, Name = "Product 1", Price = 0.5m}, 1),
                    (new ProductViewModel{Id = 2, Name = "Product 2", Price = 1.5m}, 3),

                }
            };
            const int expected_count = 4;
            var actual_count = cart_view_model.ItemCount;

            Assert.Equal(expected_count, actual_count);
        }

        [TestMethod]
        public void CartViewModel_Returns_Correct_TotalPrice()
        {
            var cart_view_model = new CartViewModel
            {
                Items = new[]
                {
                    ( new ProductViewModel { Id = 1, Name = "Product 1", Price = 0.5m }, 1 ),
                    ( new ProductViewModel { Id = 2, Name = "Product 2", Price = 1.5m }, 3 ),
                }
            };
            var expected_total_price = cart_view_model.Items.Sum(item => item.Quantity * item.product.Price);

            var actual_total_price = cart_view_model.TotalSum;

            Assert.Equal(expected_total_price, actual_total_price);
        }
    }
}
