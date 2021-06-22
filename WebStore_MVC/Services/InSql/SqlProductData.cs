using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore_MVC.Services.Interfaces;
using WebStore_MVC.ViewModels;

namespace WebStore_MVC.Services.InSql
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreDB _db;
        public SqlProductData(WebStoreDB db)
        {
            _db = db;
        }

        public IEnumerable<Brand> GetBrands() => _db.Brands;

        public IEnumerable<Section> GetSections() => _db.Sections;

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section);

            if (filter?.Ids?.Length > 0)
            {
                query = query.Where(product => filter.Ids.Contains(product.Id));
            }
            else {
                if (filter?.SectionId is { } section_id)
                    query = query.Where(p => p.SectionId == section_id);

                if (filter?.BrandId is { } brand_id)
                    query = query.Where(p => p.BrandId == brand_id);
            }
            return query;
        }

        public Product GetProductById(int id)
        {
            Product product = _db.Products
                 .Include(p => p.Brand)
                 .Include(p => p.Section)
                 .SingleOrDefault(p => p.Id == id);
            return product;
        }

        public void Update(Product product)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));
            var modifiedProduct = GetProductById(product.Id);
            modifiedProduct.Name = product.Name;
            modifiedProduct.Price = product.Price;
            _db.SaveChanges();
        }
    }
}
