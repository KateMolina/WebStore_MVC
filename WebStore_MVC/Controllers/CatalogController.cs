using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore_MVC.Services.Interfaces;

namespace WebStore_MVC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;

        public CatalogController(IProductData productData)
        {
            productData = _ProductData;
        }
        
        public IActionResult Index(int? BrandId, int? SectionId)
        {
            return View();
        }
    }
}
