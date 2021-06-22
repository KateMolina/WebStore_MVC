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
    [Authorize]
    public class UserProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Orders([FromServices] IOrderService orderService)
        {
            var orders = await orderService.GetUserOrder(User.Identity!.Name);
            return View(orders.Select(o => new UserOrderViewModel
            {
                Id = o.Id,
                Name = o.Name,
                Address = o.Address,
                Phone = o.Phone,
                TotalPrice = o.Items.Sum(item => item.TotalItemPrice)
            }));
        }
    }
}
