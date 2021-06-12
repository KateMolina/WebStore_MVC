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
            }
            catch (Exception e)
            {
                _Logger.LogInformation(e, "Error attempting to initialize products");

                throw;
            }
            try
            {
                InitializeEmployees();

            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Error attempting to initialize employees");
                throw;
            }
            _Logger.LogInformation("DB has been initialized for the total of {0}", timer.Elapsed.TotalSeconds);

        }

        public void InitializeProducts()
        {
            if (_DB.Products.Any())
            {
                _Logger.LogInformation("Products initialization skipped");
                return;
            }
            else
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
                foreach (var s in TestData.Sections) { s.Id = 0; s.ParentId = null; }
                foreach (var b in TestData.Brands) { b.Id = 0; }



                using (_DB.Database.BeginTransaction())
                {
                    _DB.Sections.AddRange(TestData.Sections);
                    _DB.Brands.AddRange(TestData.Brands);
                    _DB.AddRange(TestData.Products);
                    _DB.SaveChanges();
                    _DB.Database.CommitTransaction();
                }
                _Logger.LogInformation("Products initialization Completed for {0}", timer.Elapsed.TotalSeconds);
            }

        }
        public void InitializeEmployees()
        {
            if (_DB.Employees.Any())
            {
                _Logger.LogInformation("Employees initialization skipped");
                return;
            }
            else
            {
                var timer1 = Stopwatch.StartNew();
                foreach (var emp in TestData.Employees) { emp.Id = 0; }

                using (_DB.Database.BeginTransaction())
                {
                    _DB.Employees.AddRange(TestData.Employees);
                    _DB.SaveChanges();
                    _DB.Database.CommitTransaction();

                }
                _Logger.LogInformation("Employees initialization completed for the total of {0}", timer1.Elapsed.TotalSeconds);
            }
        }

    }
}
