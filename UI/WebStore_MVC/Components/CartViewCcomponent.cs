using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore_MVC.Services.Interfaces;

namespace WebStore_MVC.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService cartService;

        public CartViewComponent(ICartService cartService)
        {
            this.cartService = cartService;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.Count = cartService.GetCartViewModel().ItemCount;
            return View();
        }
    }
}
