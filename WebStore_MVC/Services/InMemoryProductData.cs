using System.Collections.Generic;
using WebStore.Domain.Entities;
using WebStore_MVC.Data;
using WebStore_MVC.Services.Interfaces;

namespace WebStore_MVC.Services
{
    public class InMemoryProductData:IProductData
    {

        public InMemoryProductData()
        {
        }

        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Section> GetSections() => TestData.Sections;
        

        
    }
}
