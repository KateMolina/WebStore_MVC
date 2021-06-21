using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore_MVC.Services.Interfaces;

namespace WebStore_MVC.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = Role.administrators)]
    public class ProductsController : Controller
    {
        private readonly IProductData productData;

        public ProductsController(IProductData productData)
        {
            this.productData = productData;
        }
        public IActionResult Index()
        {
            return View(productData.GetProducts());
        }

        public IActionResult Edit(int id) =>
            productData.GetProductById(id) is { } product ? View(product) : NotFound();

        

        public IActionResult Delete(int id) => 
            productData.GetProductById(id) is { } product ? View(product) : NotFound();


    }
}
