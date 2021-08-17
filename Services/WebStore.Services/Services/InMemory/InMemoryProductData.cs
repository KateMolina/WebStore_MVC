using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore_MVC.Data;
using WebStore_MVC.Services.Interfaces;

namespace WebStore_MVC.Services
{
 
    [Obsolete("Not supported anymore", true)]
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

        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public Section Getbrand(int id)
        {
            throw new NotSupportedException();
        }

        public Brand GetBrand(int id)
        {
            throw new NotSupportedException();
        }

        public Section GetSection(int id)
        {
            throw new NotImplementedException();
        }

        ProductsPage IProductData.GetProducts(ProductFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
