using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore_MVC.Services.Interfaces;

namespace WebStore_MVC.Components
{
    public class CartViewCcomponent : ViewComponent
    {
        private readonly ICartService cartService;

        public CartViewCcomponent(ICartService cartService)
        {
            this.cartService = cartService;
        }

        public IViewComponentResult Invoke() => View(cartService.GetCartViewModel());
    }
}
