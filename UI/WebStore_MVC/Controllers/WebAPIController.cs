using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Interfaces.TestAPI;

namespace WebStore_MVC.Controllers
{
    public class WebAPIController : Controller
    {
        private readonly IValuesService valuesService;

        public WebAPIController(IValuesService valuesService)
        {
            this.valuesService = valuesService;
        }

        public IActionResult Index()
        {
            var values = valuesService.GetAll();
            return View(values);
        }
    }
}
