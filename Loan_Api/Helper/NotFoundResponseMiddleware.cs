using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Loan_Api.Helper
{
    public class NotFoundResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public NotFoundResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 404)
            {
                await HandleNotFoundResponseAsync(context);
            }
        }

        private Task HandleNotFoundResponseAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 404;
            return context.Response.WriteAsync("Resource not found.");
        }
    }
}
