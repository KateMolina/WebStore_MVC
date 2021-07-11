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

namespace WebStore_MVC.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly IProductData productData;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string cartName;

       
        public InCookiesCartService(IHttpContextAccessor httpContextAccessor, IProductData productData)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.productData = productData;

            var user = httpContextAccessor.HttpContext!.User;
            var user_name = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            cartName = $"WebStore.Cart{user_name}";
        }

        private Cart Cart
        {
            get
            {
                var context = httpContextAccessor.HttpContext;
                var cookies = context!.Response.Cookies;
                var cart_cookie = context.Request.Cookies[cartName];

                if (cart_cookie is null)
                {
                    var cart = new Cart();
                    cookies.Append(cartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }
                ReplaceCookies(cookies, cart_cookie);
                return JsonConvert.DeserializeObject<Cart>(cart_cookie);
            }
            set
            {
                ReplaceCookies(httpContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
            }
        }
        private void ReplaceCookies(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(cartName);
            cookies.Append(cartName, cookie);
        }



        public void Add(int id)
        {
            var cart = Cart;
            var item = cart.CartItems.FirstOrDefault(i => i.ProductId == id);
            if (item is null) { cart.CartItems.Add(new CartItem { ProductId = id, Quantity=1}); }
            else { item.Quantity++; }
            Cart = cart;
        }

        public void Decrement(int id)
        {
            var cart = Cart;
            var item = cart.CartItems.FirstOrDefault(i => i.ProductId == id);
            if (item is null)
            { return; }

            if (item.Quantity > 0)
            { item.Quantity--; }

            if (item.Quantity <= 0)
            { cart.CartItems.Remove(item); }

            Cart = cart;
        }

        public void Remove(int id)
        {
            var cart = Cart;
            var item = cart.CartItems.FirstOrDefault(i => i.ProductId == id);
            if (item is null)
            { return; }

            cart.CartItems.Remove(item);
            Cart = cart;
        }
        public void Clear()
        {
            var cart = Cart;
            cart.CartItems.Clear();
            Cart = cart;
        }


        public CartViewModel GetCartViewModel()
        {
            var products = productData.GetProducts(new ProductFilter
            {
                Ids = Cart.CartItems.Select(i => i.ProductId).ToArray()
            });

            var products_views = products.ToView().ToDictionary(p => p.Id);

            return new CartViewModel
            {
                Items = Cart.CartItems
                .Where(item => products_views.ContainsKey(item.ProductId))
                .Select(item => (products_views[item.ProductId], item.Quantity))
            };
        }
    }
}

