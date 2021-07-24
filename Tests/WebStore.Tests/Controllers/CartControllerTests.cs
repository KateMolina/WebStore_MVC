using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Orders;
using WebStore_MVC.Controllers;
using WebStore_MVC.Services.Interfaces;
using WebStore_MVC.ViewModels;
using Assert = Xunit.Assert;


namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {

        [TestMethod]
        public async Task Checkout_Invalid_ModelState_Returns_ViewModel()
        {
            const string name = "TestOrder";
            var cart_service_mock = new Mock<ICartService>();
            var order_service_mock = new Mock<IOrderService>();

            var controller = new CartController(cart_service_mock.Object);
            controller.ModelState.AddModelError("error", "InvalidViewVodel");
            var order_model = new OrderViewModel
            {
                Name = name

            };

            var result = await controller.Checkout(order_model, order_service_mock.Object);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CartOrderViewModel>(viewResult.Model);

            Assert.Equal(name, model.Order.Name);


        }
        [TestMethod]
        public async Task Checkout_Valid_ModelState_Call_Service_And_Returns_Redirect()
        {
            const int expected_orderId = 1;
            const string expected_user = "test user";
            const string expected_orderName = "test order";
            const string expected_address = "test address";
            const string expected_phone = "test number";
            

            var cart_service_mock = new Mock<ICartService>();
            cart_service_mock.Setup(c => c.GetCartViewModel())
                .Returns(new CartViewModel
                {
                    Items = new[] { (new ProductViewModel { Name = "testProduct" }, 1) }
                });
            var order_service_mock = new Mock<IOrderService>();
            order_service_mock.Setup(o => o.CreateOrder(It.IsAny<string>(), It.IsAny<CartViewModel>(), It.IsAny<OrderViewModel>()))
                .ReturnsAsync(new Order
                {
                    Id = expected_orderId,
                    Name = expected_orderName,
                    Address = expected_address,
                    Phone = expected_phone,
                    Items = Array.Empty<OrderItem>()
                });



            var controller = new CartController(cart_service_mock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, expected_user) }))
                    }
                }
            };
            var order_model = new OrderViewModel
            {
                Name = expected_orderName,
                Address = expected_address,
                Phone = expected_phone
            };

            var result = await controller.Checkout(order_model, order_service_mock.Object);
            var redirect_res = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(CartController.OrderConfirmed), redirect_res.ActionName);
            Assert.Null(redirect_res.ControllerName);
            Assert.Equal(expected_orderId, redirect_res.RouteValues["id"]);
        }

    }
}
