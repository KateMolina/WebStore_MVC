using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebStore_MVC.Infrastructure.Mapping;
using WebStore_MVC.Models;
using WebStore_MVC.Services.Interfaces;
using WebStore_MVC.ViewModels;

namespace WebStore_MVC.Controllers
{
    public class HomeController : Controller
    {

       // private readonly ILogger<HomeController> _logger;
        //public object index;
        private readonly IConfiguration config;

        public HomeController(/*ILogger<HomeController> logger*/IConfiguration config)
        {
            this.config = config;
            //_logger = logger;
        }

        public IActionResult Index([FromServices]IProductData _ProductData)
        {
            var products = _ProductData.GetProducts().Products.Take(9).ToView();
            ViewBag.Products = products;
            return View();
        }
        public IActionResult Throw(string message) => throw new ApplicationException(message ?? "Error in Home Controller");

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Blog() => View();

        public IActionResult BlogSingle() => View();

        public IActionResult NotFound404() => View();

        //public IActionResult Cart() => View();

        public IActionResult Checkout() => View();

        public IActionResult ContactUs() => View();

      //  public IActionResult Login() => View();




        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
