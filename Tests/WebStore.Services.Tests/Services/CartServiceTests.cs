using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assert = Xunit.Assert;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebStore.Domain.Entities;
using WebStore_MVC.ViewModels;
using WebStore_MVC.Services.Interfaces;
using Moq;
using WebStore.Interfaces.Services;
using WebStore.Domain;
//using WebStore_MVC.Services.Services;
using System.Diagnostics;
using WebStore_MVC.Services.Services;

namespace WebStore.Services.Tests.Services
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _Cart;
        private ICartService cartService;

        private Mock<IProductData> productDataMock;
        private Mock<ICartStorage> cartStorageMock;


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

            productDataMock = new Mock<IProductData>();
            productDataMock.Setup(c => c.GetProducts(It.IsAny<ProductFilter>()))
                .Returns(new ProductsPage(new[]
                {
                    new Product
                    {
                        Id = 1,
                            Name = "Product 1",
                            Price = 1.1m,
                            Order = 1,
                            ImageUrl = "img_1.png",
                            Brand = new Brand { Id = 1, Name = "Brand 1", Order = 1},
                            SectionId = 1,
                            Section = new Section{ Id = 1, Name = "Section 1", Order = 1 },

                    },
                    new Product
                        {
                            Id = 2,
                            Name = "Product 2",
                            Price = 2.2m,
                            Order = 2,
                            ImageUrl = "img_2.png",
                            Brand = new Brand { Id = 2, Name = "Brand 2", Order = 2},
                            SectionId = 2,
                            Section = new Section{ Id = 2, Name = "Section 2", Order = 2 },
                        },
                        new Product
                        {
                            Id = 3,
                            Name = "Product 3",
                            Price = 3.3m,
                            Order = 3,
                            ImageUrl = "img_3.png",
                            Brand = new Brand { Id = 3, Name = "Brand 3", Order = 3},
                            SectionId = 3,
                            Section = new Section{ Id = 3, Name = "Section 3", Order = 3 },
                        },
                }, 3));

            cartStorageMock = new Mock<ICartStorage>();
            cartStorageMock.Setup(c => c.Cart).Returns(_Cart);

            cartService = new CartService(cartStorageMock.Object, productDataMock.Object);

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

        [TestMethod]
        public void CartService_Add_AddsCorrectly()
        {
            _Cart.CartItems.Clear();
            const int expectedId = 5;
            const int expected_items_count = 1;

            cartService.Add(expectedId);
            Assert.Equal(expected_items_count, _Cart.ItemsCount);
            Assert.Single(_Cart.CartItems);
            Assert.Equal(expectedId, _Cart.CartItems.First().ProductId);
        }

        [TestMethod]
        public void CartService_Remove_Correct_Item()
        {
            const int item_id = 1;
            const int expected_product_id = 2;

            cartService.Remove(item_id);
            Assert.Single(_Cart.CartItems);
            Assert.Equal(expected_product_id, _Cart.CartItems.Single().ProductId);

        }
        [TestMethod]
        public void CartService_Clear_ClearCart()
        {
            cartService.Clear();

            Assert.Empty(_Cart.CartItems);
        }
        [TestMethod]
        public void CartService_Decrement_Correct()
        {
            const int item_id = 2;
            const int expected_quantity = 2;
            const int expected_items_count = 3;
            const int expected_products_count = 2;

            cartService.Decrement(item_id);

            Assert.Equal(expected_items_count, _Cart.ItemsCount);
            Assert.Equal(expected_products_count, _Cart.CartItems.Count);
            var items = _Cart.CartItems.ToArray();
            Assert.Equal(item_id, items[1].ProductId);
            Assert.Equal(expected_quantity, items[1].Quantity);
        }

        [TestMethod]
        public void CartService_Remove_Item_When_Decrement_to_0()
        {
            const int item_id = 1;
            const int expected_items_count = 3;

            cartService.Decrement(item_id);

            Assert.Equal(expected_items_count, _Cart.ItemsCount);
            Assert.Single(_Cart.CartItems);
        }

        [TestMethod]
        public void CartService_GetViewModel_WorkCorrect()
        {
            Debug.WriteLine("Тестирование преобразования корзины в модель-представления");

            const int expected_items_count = 4;
            const decimal expected_first_product_price = 1.1m;

            var result = cartService.GetCartViewModel();

            Assert.Equal(expected_items_count, result.ItemCount);
            Assert.Equal(expected_first_product_price, result.Items.First().product.Price);
        }
    }
}

