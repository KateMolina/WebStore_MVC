using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore_MVC.Services.Interfaces;
using WebStore_MVC.ViewModels;

namespace WebStore_MVC.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BrandsViewComponent(IProductData productData)
        {
            _ProductData = productData;
        }

        public IViewComponentResult Invoke(string BrandId)
        {
            ViewBag.BrandId = int.TryParse(BrandId, out var id) ? id : (int?)null;

           return View(GetBrands());
        }


        public IEnumerable<BrandViewModel> GetBrands() =>

            _ProductData.GetBrands().OrderBy(b => b.Order).Select(b => new BrandViewModel
            {
                Id = b.Id,
                Name = b.Name,

            });

    }
}
