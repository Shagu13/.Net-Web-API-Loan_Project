using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Loan_Api.Helper
{
    public class UnauthorizedResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public UnauthorizedResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 401)
            {
                await HandleUnauthorizedResponseAsync(context);
            }
        }

        private Task HandleUnauthorizedResponseAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 401;
            return context.Response.WriteAsync("Unauthorized: JWT token is missing or invalid.");
        }
    }
}
