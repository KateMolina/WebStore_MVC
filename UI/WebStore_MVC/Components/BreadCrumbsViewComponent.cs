using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore_MVC.Services.Interfaces;
using WebStore_MVC.ViewModels;

namespace WebStore_MVC.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductData productData;

        public BreadCrumbsViewComponent(IProductData productData)
        {
            this.productData = productData;
        }

        public IViewComponentResult Invoke()
        {
            var model = new BreadCrumbsViewModel();

            if(int.TryParse(Request.Query["SectionId"], out var section_id))
            {
                model.Section = productData.GetSection(section_id);
                if (model.Section?.ParentId is { } parent_section_id)
                    model.Section.Parent = productData.GetSection(parent_section_id);
            }

            if(int.TryParse(Request.Query["BrandId"], out var brand_id))
            {
                model.Brand = productData.GetBrand(brand_id);
            }

            if(int.TryParse(ViewContext.RouteData.Values["id"]?.ToString(), out var product_id))
            {
                model.Product = productData.GetProductById(product_id)?.Name;
            }

            return View(model);
        }
    }
}
