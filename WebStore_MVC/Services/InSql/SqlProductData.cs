﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore_MVC.Services.Interfaces;

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
            IQueryable<Product> query = _db.Products;

            if (filter?.SectionId is { } section_id)
            {
                query = query.Where(p => p.SectionId == section_id);
            }

            if (filter?.BrandId is { } brand_id)
            {
                query = query.Where(p => p.BrandId == brand_id);
            }
            return query;
        }


    }
}