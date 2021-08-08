using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore_MVC.Services.Interfaces;
using WebStore_MVC.ViewModels;

namespace WebStore_MVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }
        public IActionResult Index() => View(new CartOrderViewModel { Cart = cartService.GetCartViewModel() });


        public IActionResult Add(int id)
        {
            cartService.Add(id);
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Remove(int id)
        {
            cartService.Remove(id);
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Decrement(int id)
        {
            cartService.Decrement(id);
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Clear()
        {
            cartService.Clear();
            return RedirectToAction("Index", "Cart");
        }
        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(OrderViewModel OrderModel, [FromServices] IOrderService OrderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), new CartOrderViewModel
                {
                    Cart = cartService.GetCartViewModel(),
                    Order = OrderModel,
                });
            var order = await OrderService.CreateOrder(
                User.Identity!.Name,
                cartService.GetCartViewModel(),
                OrderModel);

            cartService.Clear();

            return RedirectToAction(nameof(OrderConfirmed), new { order.Id});
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        #region Web-API for js

        public IActionResult GetCartView() => ViewComponent("Cart");

        public IActionResult AddAPI(int id)
        {
            cartService.Add(id);
            return Json(new { id, message = $"Product {id} has been added to the cart"});
        }
        public IActionResult RemoveAPI(int id)
        {
            cartService.Remove(id);
            return Ok(new { id, message = $"Product {id} has been removed from the cart" });
        }
        public IActionResult DecrementAPI(int id)
        {
            cartService.Decrement(id);
            return Ok();
        }

        public IActionResult ClearAPI()
        {
            cartService.Clear();
            return Ok();
        }

        #endregion
    }


}
