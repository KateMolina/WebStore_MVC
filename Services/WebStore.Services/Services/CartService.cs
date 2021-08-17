using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore_MVC.Services.Interfaces;
using WebStore_MVC.ViewModels;
using Newtonsoft.Json;
using WebStore.Domain;
using WebStore_MVC.Infrastructure.Mapping;
using WebStore.Interfaces.Services;

namespace WebStore_MVC.Services.Services
{
    public class CartService : ICartService
    {
        private readonly ICartStorage cartStorage;
        private readonly IProductData productData;
     

       
        public CartService(ICartStorage cartStorage, IProductData productData)
        {
            this.cartStorage = cartStorage;
            this.productData = productData;

            
        }

     

        public void Add(int id)
        {
            var cart = cartStorage.Cart;
            var item = cart.CartItems.FirstOrDefault(i => i.ProductId == id);
            if (item is null) { cart.CartItems.Add(new CartItem { ProductId = id, Quantity=1}); }
            else { item.Quantity++; }
            cartStorage.Cart = cart;
        }

        public void Decrement(int id)
        {
            var cart = cartStorage.Cart;
            var item = cart.CartItems.FirstOrDefault(i => i.ProductId == id);
            if (item is null)
            { return; }

            if (item.Quantity > 0)
            { item.Quantity--; }

            if (item.Quantity <= 0)
            { cart.CartItems.Remove(item); }

            cartStorage.Cart = cart;
        }

        public void Remove(int id)
        {
            var cart = cartStorage.Cart;
            var item = cart.CartItems.FirstOrDefault(i => i.ProductId == id);
            if (item is null)
            { return; }

            cart.CartItems.Remove(item);
            cartStorage.Cart = cart;
        }
        public void Clear()
        {
            var cart = cartStorage.Cart;
            cart.CartItems.Clear();
            cartStorage.Cart = cart;
        }


        public CartViewModel GetCartViewModel()
        {
            var products = productData.GetProducts(new ProductFilter
            {
                Ids = cartStorage.Cart.CartItems.Select(i => i.ProductId).ToArray()
            });

            var products_views = products.Products.ToView().ToDictionary(p => p.Id);

            return new CartViewModel
            {
                Items = cartStorage.Cart.CartItems
                .Where(item => products_views.ContainsKey(item.ProductId))
                .Select(item => (products_views[item.ProductId], item.Quantity))
            };
        }
    }
}

