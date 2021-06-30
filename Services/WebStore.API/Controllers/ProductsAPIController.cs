using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Interfaces;
using WebStore_MVC.Services.Interfaces;

namespace WebStore.API.Controllers
{
    [ApiController]
    [Route(WebAPIAddresses.Products)]
    public class ProductsAPIController : Controller
    {
        private readonly IProductData productData;

        public ProductsAPIController(IProductData productData)
        {
            this.productData = productData;
        }

        [HttpGet("sections")]
        public IActionResult GetSections()
        {
            return Ok(productData.GetSections());
        }

        [HttpGet("sections/{id:int}")]
        public IActionResult GetSection(int id) => Ok(productData.GetBrand(id));

        [HttpGet("brands")]
        public IActionResult GetBrands()
        {
            return Ok(productData.GetBrands());
        }

        [HttpGet("brands/{id:int}")]
        public IActionResult GetBrand(int id) => Ok(productData.GetBrand(id));
        [HttpPost]
        public IActionResult GetProducts(ProductFilter filter = null) => Ok(productData.GetProducts(filter));

        [HttpGet("{id:int}")]
        public IActionResult GetProduct(int id) => Ok(productData.GetProductById(id));
    }
}
