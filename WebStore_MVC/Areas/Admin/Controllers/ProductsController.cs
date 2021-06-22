using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore_MVC.Services.InSql;
using WebStore_MVC.Services.Interfaces;
using WebStore_MVC.ViewModels;

namespace WebStore_MVC.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = Role.administrators)]
    public class ProductsController : Controller
    {
        private readonly IProductData productData;
        private readonly WebStoreDB db;

        public ProductsController(IProductData productData, WebStoreDB db)
        {
            this.productData = productData;
            this.db = db;
        }
        public IActionResult Index()
        {
            return View(productData.GetProducts());
        }

        public IActionResult Edit(int id)
        {
            var product = productData.GetProductById(id);
            if (product is null) { NotFound(); }

            return View(new ProductViewModel
            {
                Name = product.Name,
                Price = product.Price
            });
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel productViewModel)
        {
            var item = new Product
            {
                Id = productViewModel.Id,
                Name = productViewModel.Name,
                Price = productViewModel.Price,
                ImageUrl = productViewModel.ImageUrl,
            };
            productData.Update(item);

            return RedirectToAction("Index");
        }



        public IActionResult Delete(int id) =>
            productData.GetProductById(id) is { } product ? View(product) : NotFound();


    }
}
