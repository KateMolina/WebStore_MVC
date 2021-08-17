using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Domain;
using WebStore_MVC.Infrastructure.Mapping;
using WebStore_MVC.Services.Interfaces;
using WebStore_MVC.ViewModels;

namespace WebStore_MVC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly IConfiguration configuration;

        public CatalogController(IProductData productData, IConfiguration configuration)
        {
            _ProductData = productData;
            this.configuration = configuration;
        }

        public IActionResult Index(int? BrandId, int? SectionId, int Page =1, int? PageSize = null)
        {
            var page_size = PageSize ?? (int.TryParse(configuration["CatalogPageSize"], out var value)
                ? value : null);

            var filter = new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Page = Page,
                PageSize = page_size,
            };

            var products = _ProductData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = products
                .OrderBy(p => p.Order)
                .ToView()
            }); ;
        }


       public IActionResult ProductDetails(int id)
        {
            var product = _ProductData.GetProductById(id);
            if (product is null) return NotFound();

            return View(product.ToView());
        }

    }
}
