using AronGroup.Infrastructures.Data.DbContext;
using AronGroup.Infrastructures.Data.Seeds.Abstractions;

namespace AronGroup.Extensions;

public static class MigrateManagerExtensions
{
    public static WebApplication MigrateDatabase(this WebApplication webApp)
    {
        using (var scope = webApp.Services.CreateScope())
        {
            using (var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
                try
                {
                    appContext.Database.EnsureCreated();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        return webApp;
    }

    public static void IntializeDatabase(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var dataInitializers = scope.ServiceProvider.GetServices<IDataInitializer>();
            foreach (var dataInitializer in dataInitializers)
                dataInitializer.InitializeData();
        }
    }
}
