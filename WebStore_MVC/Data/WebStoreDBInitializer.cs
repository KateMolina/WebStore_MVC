using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;

namespace WebStore_MVC.Data
{
    public class WebStoreDBInitializer
    {
        private readonly WebStoreDB _DB;
        private readonly ILogger<WebStoreDBInitializer> _Logger;
        public WebStoreDBInitializer(WebStoreDB db, ILogger<WebStoreDBInitializer>logger)
        {
            _DB = db;
            _Logger = logger;
        }
        public void Initialize()
        {
            _Logger.LogInformation("Initialization has started...");

            if (_DB.Database.GetPendingMigrations().Any())
            {
                _Logger.LogInformation("DB Migration is running...");
                _DB.Database.Migrate();
                _Logger.LogInformation("DB Migration completed.");
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
                _Logger.LogInformation(e,"Error attempting to initialize products");

                throw;
            }
            _Logger.LogInformation("DB Initialization has completed");

        }

        public void InitializeProducts()
        {
            if (_DB.Products.Any())
            {
                _Logger.LogInformation("The DB doesn't need to initialize products");
                return;
            }
            _Logger.LogInformation("Sections initialization skipped");
           
                using (_DB.Database.BeginTransaction())
                {
                _DB.AddRange(TestData.Sections);
                _DB.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] ON");
                _DB.SaveChanges();
                _DB.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] OFF");
                _DB.Database.CommitTransaction();
            }
            _Logger.LogInformation("Sections initialization Completed");

            _Logger.LogInformation("Brands initialization skipped");

            using (_DB.Database.BeginTransaction())
            {
                _DB.AddRange(TestData.Brands);
                _DB.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] ON");
                _DB.SaveChanges();                                    
                _DB.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] OFF");
                _DB.Database.CommitTransaction();
            }
            _Logger.LogInformation("Brands initialization Completed");

            _Logger.LogInformation("Products initialization skipped");

            using (_DB.Database.BeginTransaction())
            {
                _DB.AddRange(TestData.Products);
                _DB.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");
                _DB.SaveChanges();                                    
                _DB.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");
                _DB.Database.CommitTransaction();
            }
            _Logger.LogInformation("Products initialization Completed");

        }

    }
}
