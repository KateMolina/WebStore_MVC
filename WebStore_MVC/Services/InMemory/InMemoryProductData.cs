using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore_MVC.Data;
using WebStore_MVC.Services.Interfaces;

namespace WebStore_MVC.Services
{
    public class InMemoryProductData:IProductData
    {

        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Section> GetSections() => TestData.Sections;

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            IEnumerable<Product> query = TestData.Products;

            if (filter?.SectionId is { } section_id)
                query = query.Where(product => product.SectionId == section_id);

            if (filter?.BrandId is { } brand_id)
                query = query.Where(product => product.BrandId == brand_id);

            return query;
        }

        public Product GetProductById(int id) => TestData.Products.SingleOrDefault(p => p.Id ==id);

        public void Update(Product item)
        {
            throw new System.NotImplementedException();
        }
    }
}
