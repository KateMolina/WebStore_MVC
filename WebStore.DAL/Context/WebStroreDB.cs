using System;
using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities;

namespace WebStore.DAL.Context
{
    public class WebStroreDB:DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Brand> Brands { get; set; }

        DbSet<Section> Sections { get; set; }

        public WebStroreDB(DbContextOptions<WebStroreDB> options):base(options)
        {
            //git 1.5
        }
    }
}
 