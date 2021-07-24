using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.InCookies
{
    public class InCookiesCartStorage : ICartStorage
    {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string cartName;

        public InCookiesCartStorage(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            var user = httpContextAccessor.HttpContext!.User;
            var user_name = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            cartName = $"WebStore.Cart{user_name}";
        }
        public Cart Cart
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
    }
}
