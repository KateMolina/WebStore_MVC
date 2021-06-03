using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors.Infrastructure;
using WebStore.Domain.Entities;

namespace WebStore_MVC.Services.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        IEnumerable<Brand> GetBrands();


    }
}
