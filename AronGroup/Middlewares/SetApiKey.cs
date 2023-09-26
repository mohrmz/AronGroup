using AronGroup.Models.Common;

namespace AronGroup.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SetApiKey
    {
        private readonly RequestDelegate _next;

        public SetApiKey(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var options = new ApiAuthenticationOptions();
            var token = httpContext.Session.GetString("ApiKey");
            if (!string.IsNullOrEmpty(token) && string.IsNullOrEmpty(httpContext.Request.Headers[options.TokenHeaderName]))
            {
                httpContext.Request.Headers.Add(options.TokenHeaderName, $"{ApiAuthenticationOptions.DefaultScheme} {token}");
            }

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SetApiKeyExtensions
    {
        public static IApplicationBuilder UseSetApiKey(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SetApiKey>();
        }
    }
}
