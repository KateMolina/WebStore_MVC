using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.WebAPI.Clients.Base;
using WebStore_MVC.Services.Interfaces;

namespace WebStore.WebAPI.Clients.Products
{
    class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(HttpClient client) : base(client, WebAPIAddresses.Products) { }


        public Brand GetBrand(int id) => Get<Brand>($"{Address}/brands/{id}");
        

        public IEnumerable<Brand> GetBrands() => Get<IEnumerable<Brand>>($"{Address}/brands");
       

        public Product GetProductById(int id)
        {
            return Get<Product>($"{Address}/{id}");
        }

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            var response = Post(Address, filter);
            var result = response.Content.ReadFromJsonAsync<IEnumerable<Product>>().Result;
            return result;
        }

        public Section GetSection(int id) => Get<Section>($"{Address}/sections/{id}");
        

        public IEnumerable<Section> GetSections() => Get<IEnumerable<Section>>($"{Address}/sections");
       

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Product item)
        {
            throw new NotImplementedException();
        }
    }
}
