using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebStore_MVC.Infrastructure
{
    public class TestMiddleWare
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<TestMiddleWare> _Logger;

        public TestMiddleWare(RequestDelegate next, ILogger<TestMiddleWare> Logger)
        {
            _Next = next;
            _Logger = Logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // обработка контекста
            var processing = _Next(context);

            //do work while the rest part of the conveyer is processing smth...

            await processing;
        }


    }
}
