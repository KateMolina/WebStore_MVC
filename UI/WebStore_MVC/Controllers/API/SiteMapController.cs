using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore_MVC.Services.Interfaces;

namespace WebStore_MVC.Controllers.API
{

    public class SiteMapController : ControllerBase
    {


        public IActionResult Index([FromServices] IProductData productData)
        {
            var nodes = new List<SitemapNode>
            {
                new(Url.Action("Index", "Home")),
                new(Url.Action("Blog", "Home")),
                new(Url.Action("Index", "Catalog")),
                new(Url.Action("Index", "WebAPI"))
            };

            nodes.AddRange(productData.GetSections().Select(section => new SitemapNode(Url.Action("Index", "Catalog",
                new { SectionId = section.Id }))));

            foreach(var brand in productData.GetBrands())
            {
                nodes.Add(new SitemapNode(Url.Action("Index", "Catalog", new { BrandId = brand.Id })));
            }
            foreach (var product in productData.GetProducts())
            {
                nodes.Add(new SitemapNode(Url.Action("ProductDetails", "Catalog", new { product.Id })));
            }

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }

    }
}
