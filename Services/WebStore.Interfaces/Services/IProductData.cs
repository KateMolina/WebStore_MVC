using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore_MVC.Services.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        Section GetSection(int id);

        IEnumerable<Brand> GetBrands();
        Brand GetBrand(int id);

        IEnumerable<Product> GetProducts(ProductFilter filter = null);
        Product GetProductById(int id);
        void Update(Product item);
        void Remove(int id);
    }
}
