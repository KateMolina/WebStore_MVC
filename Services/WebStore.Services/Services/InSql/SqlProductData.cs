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
using Microsoft.Extensions.Logging;
using WebStore.Domain.DTO;

namespace WebStore_MVC.Services.InSql
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<SqlProductData> logger;

        public SqlProductData(WebStoreDB db, ILogger<SqlProductData> logger)
        {
            _db = db;
            this.logger = logger;
        }

        public IEnumerable<Brand> GetBrands() => _db.Brands;

        public IEnumerable<Section> GetSections() => _db.Sections.Include(p=>p.Products);

        public ProductsPage GetProducts(ProductFilter filter = null)
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

            var total_count = query.Count();

            if (filter is { PageSize: > 0 and var page_size, Page: > 0 and var page_number })
                query = query
                    .Skip((page_number - 1) * page_size)
                    .Take(page_size);
            return new ProductsPage(query.AsEnumerable(), total_count);
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
            logger.LogInformation($"Editing id: {product.Id} started");
            if (product is null) throw new ArgumentNullException(nameof(product));
            var modifiedProduct = GetProductById(product.Id);
            modifiedProduct.Name = product.Name;
            modifiedProduct.Price = product.Price;
            logger.LogInformation($"Item id: {product.Id} has been modified");
            _db.SaveChanges();
        }

        public void Remove(int id)
        {
            logger.LogInformation($"Deleting id: {id} started");
            logger.LogInformation($"Getting product id: {id}");
            var product = GetProductById(id);
            _db.Products.Remove(product);
            logger.LogInformation($"Item id: {id} has been removed from the db");
            _db.SaveChanges();
        }

        public Section Getbrand(int id) => _db.Sections.Include(s => s.Products).FirstOrDefault(s => s.Id == id);


        public Brand GetBrand(int id)=>_db.Brands.Include(b => b.Products).FirstOrDefault(b => b.Id == id);

        public Section GetSection(int id)
        {
            throw new NotImplementedException();
        }
    }
}