namespace MIS.WebApp.Middlewares
{
    using Microsoft.AspNetCore.Builder;

    public static class SeedDataMiddlewareExtensions
    {
        public static IApplicationBuilder UseSeedDataMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedDataMiddleware>();
        }
    }
}