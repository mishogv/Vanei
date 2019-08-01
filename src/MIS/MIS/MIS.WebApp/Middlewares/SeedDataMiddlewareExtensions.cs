namespace MIS.WebApp.Middlewares
{
    using System.Linq;
    using System.Reflection;

    using Data;
    using Data.Seeding;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SeedDataMiddlewareExtensions
    {
        public static void UseSeedData(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider
                                          .GetRequiredService<MISDbContext>();

                context.Database.EnsureCreated();

                Assembly.GetAssembly(typeof(MISDbContext))
                        .GetTypes()
                        .Where(type => typeof(ISeeder).IsAssignableFrom(type))
                        .Where(type => type.IsClass)
                        .Select(type => (ISeeder)serviceScope.ServiceProvider.GetRequiredService(type))
                        .ToList()
                        .ForEach(seeder => seeder.Seed().GetAwaiter().GetResult());
            }
        }
    }
}