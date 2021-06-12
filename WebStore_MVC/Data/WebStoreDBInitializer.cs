using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;

namespace WebStore_MVC.Data
{
    public class WebStoreDBInitializer
    {
        private readonly WebStoreDB _DB;
        private readonly ILogger<WebStoreDBInitializer> _Logger;
        public WebStoreDBInitializer(WebStoreDB db, ILogger<WebStoreDBInitializer> logger)
        {
            _DB = db;
            _Logger = logger;
        }
        public void Initialize()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Initialization has started...");

            if (_DB.Database.GetPendingMigrations().Any())
            {
                _Logger.LogInformation("DB Migration is running...");
                _DB.Database.Migrate();
                _Logger.LogInformation("DB Migration completed for {}", timer.Elapsed.TotalSeconds);
            }
            else
            {
                _Logger.LogInformation("DB Migration Not Needed.");
            }

            try
            {
                InitializeProducts();
                InitializeEmployees();
            }
            catch (Exception e)
            {
                _Logger.LogInformation(e, "Error attempting to initialize products");

                throw;
            }
            _Logger.LogInformation("DB Initialization has completed fot the total of {}", timer.Elapsed.TotalSeconds);

        }

        public void InitializeProducts()
        {
            var timer = Stopwatch.StartNew();

            var sections_pool = TestData.Sections.ToDictionary(section => section.Id);
            var brands_pool = TestData.Brands.ToDictionary(brand => brand.Id);

            foreach (var section in TestData.Sections.Where(s => s.ParentId != null))
                section.Parent = sections_pool[(int)section.ParentId!];

            foreach (var product in TestData.Products)
            {
                product.Section = sections_pool[product.SectionId];
                if (product.BrandId is { } brand_id)
                {
                    product.Brand = brands_pool[brand_id];
                }
                product.Id = 0;
                product.SectionId = 0;
                product.BrandId = null; 
            }
            foreach(var s in TestData.Sections) { s.Id = 0; s.ParentId = null; }
            foreach(var b in TestData.Brands) { b.Id = 0; }

            _Logger.LogInformation("Products initialization skipped");

            using (_DB.Database.BeginTransaction())
            {
                _DB.Sections.AddRange(TestData.Sections);
                _DB.Brands.AddRange(TestData.Brands);
                _DB.AddRange(TestData.Products);
                _DB.SaveChanges();
                _DB.Database.CommitTransaction();
            }
            _Logger.LogInformation("Products initialization Completed for {}", timer.Elapsed.TotalSeconds);

        }
        public void InitializeEmployees()
        {
           foreach(var emp in TestData.Employees) { emp.Id = 0; }

            using (_DB.Database.BeginTransaction())
            {
                _DB.Employees.AddRange(TestData.Employees);
                _DB.SaveChanges();
                _DB.Database.CommitTransaction();

            }
        }

    }
}
