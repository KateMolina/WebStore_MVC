using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore_MVC.Infrastructure.MiddleWare
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate Next, ILogger<ErrorHandlingMiddleware> Logger)
        {
            _next = Next;
            _logger = Logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                ExceptionHandler(e, context);
                throw;
            }
        }

        private void ExceptionHandler(Exception error, HttpContext context)
        {
            _logger.LogError(error, "Error calling {0}", context.Request.Path);
        }

    }
}
