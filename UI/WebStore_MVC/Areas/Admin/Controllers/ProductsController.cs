using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore_MVC.Infrastructure.Mapping;
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
        private readonly ILogger<ProductsController> logger;

        public ProductsController(IProductData productData, ILogger<ProductsController> logger)
        {
            this.productData = productData;
            this.logger = logger;
        }
        public IActionResult Index()
        {
            return View(productData.GetProducts().Products);
        }

        public IActionResult Edit(int id)
        {
            logger.LogInformation("Modifying product id: {0} has started", id);
            var product = productData.GetProductById(id);
            if (product is null) { NotFound(); }

            logger.LogInformation("Editing product of id: {0} is in progress, viewmodel created", id);

            return View(new ProductViewModel
            {
                Name = product.Name,
                Price = product.Price,
            });
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel productViewModel)
        {
            logger.LogInformation("Editing product...");

            var item = new Product
            {
                Id = productViewModel.Id,
                Name = productViewModel.Name,
                Price = productViewModel.Price,
                ImageUrl = productViewModel.ImageUrl,
            };
            productData.Update(item);

            logger.LogInformation("Product of id: {0} has been modified", item.Id);
            return RedirectToAction("Index");
        }



        public IActionResult Delete(int id)
        {
            var product = productData.GetProductById(id);
            if (product is null) { NotFound(); }

            return View(product.ToView());
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            logger.LogInformation($"Deleting  item id: {id}");
            productData.Remove(id);

            logger.LogInformation($"Item id: {id} has been removed");
            return RedirectToAction("Index");
        }



    }
}
