using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.WebAPI.Clients.Base;
using WebStore_MVC.Services.Interfaces;

namespace WebStore.WebAPI.Clients.Products
{
   public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(HttpClient client) : base(client, WebAPIAddresses.Products) { }


        public Brand GetBrand(int id) => Get<BrandDTO>($"{Address}/brands/{id}").FromDTO();
        

        public IEnumerable<Brand> GetBrands() => Get<IEnumerable<BrandDTO>>($"{Address}/brands").FromDTO();
       

        public Product GetProductById(int id)
        {
            return Get<ProductDTO>($"{Address}/{id}").FromDTO();
        }

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            var response = Post(Address, filter ?? new ProductFilter());
            var result = response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>().Result;
            return result.FromDTO();
        }

        public Section GetSection(int id) => Get<SectionDTO>($"{Address}/sections/{id}").FromDTO();
        

        public IEnumerable<Section> GetSections() => Get<IEnumerable<SectionDTO>>($"{Address}/sections").FromDTO();
       

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
