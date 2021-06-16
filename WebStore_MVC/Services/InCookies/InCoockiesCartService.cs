using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore_MVC.Services.Interfaces;
using WebStore_MVC.ViewModels;
using Newtonsoft.Json;

namespace WebStore_MVC.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly HttpContextAccessor httpContextAccessor;
        private readonly IProductData productData;
        private readonly string cartName;

        public InCookiesCartService(HttpContextAccessor httpContextAccessor, IProductData productData)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.productData = productData;

            var user = httpContextAccessor.HttpContext.User;
            var user_name = user.Identity.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            cartName = $"WebStore. Cart {user_name}";
        }

        private Cart Cart
        {
            get
            {
                var context = httpContextAccessor.HttpContext;
                var cookies = context.Response.Cookies;
                var cart_cookie = context.Request.Cookies[cartName];

                if(cart_cookie is null)
                {
                    var cart = new Cart();
                    cookies.Append(cart_cookie, JsonConvert.SerializeObject(cart));
                    return cart;
                }
                ReplaceCookies(cookies, cart_cookie);
                return JsonConvert.DeserializeObject<Cart>(cart_cookie);
            }

            set
            {
                ReplaceCookies(httpContextAccessor.HttpContext.Response.Cookies, JsonConvert.SerializeObject(value));
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
            var item = cart.cartItems.FirstOrDefault(i => i.ProductId == id);
            if(item is null) { cart.cartItems.Add(new CartItem { ProductId = id }); }
            else { item.Quantity++; }
            Cart = cart;
        }

        public void Decrement(int id)
        {
            var cart = Cart;
            var item = cart.cartItems.FirstOrDefault(i => i.ProductId == id);
            if(item is null) 
            { return; }

            if (item.Quantity > 0) 
            { item.Quantity--; }

            if (item.Quantity <= 0) 
            { cart.cartItems.Remove(item); }

            Cart = cart;
        }

        public void Remove(int id)
        {
            var cart = Cart;
            var item = cart.cartItems.FirstOrDefault(i => i.ProductId == id);
            if (item is null)
            { return; }

            cart.cartItems.Remove(item);
            Cart = cart;
        }
        public void Clear()
        {
            var cart = Cart;
            cart.cartItems.Clear();
            Cart = cart;
        }


        public CartViewModel GetCartViewModel()
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
