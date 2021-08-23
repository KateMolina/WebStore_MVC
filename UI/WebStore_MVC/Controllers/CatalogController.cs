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

        private const string _PageSizeConfigName = "CatalogPageSize";

        public CatalogController(IProductData productData, IConfiguration configuration)
        {
            _ProductData = productData;
            this.configuration = configuration;
        }

        public IActionResult Index(int? BrandId, int? SectionId, int Page = 1, int? PageSize = null)
        {
            var page_size = PageSize ?? configuration.GetValue(_PageSizeConfigName, 6);


            var filter = new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Page = Page,
                PageSize = page_size,
            };

            var (products, total_count) = _ProductData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = products
                .OrderBy(p => p.Order)
                .ToView(),
                PageViewModel = new PageViewModel
                {
                    Page = Page,
                    PageSize = page_size,
                    TotalItems = total_count,
                },
            }); ;
        }


        public IActionResult ProductDetails(int id)
        {
            var product = _ProductData.GetProductById(id);
            if (product is null) return NotFound();

            return View(product.ToView());
        }

        public IActionResult GetFeaturedItems(int? BrandId, int? SectionId, int Page = 1, int? PageSize = null) =>
            PartialView("Partial/_FeaturedItems", GetProducts(BrandId, SectionId, Page, PageSize));

        private IEnumerable<ProductViewModel> GetProducts(int? BrandId, int? SectionId, int Page, int? PageSize) =>

             _ProductData.GetProducts(
                new ProductFilter
                {
                    BrandId = BrandId,
                    SectionId = SectionId,
                    Page = Page,
                    PageSize = PageSize ?? configuration.GetValue(_PageSizeConfigName, 6)
                }).Products.OrderBy(p => p.Order).ToView();
    }

}
