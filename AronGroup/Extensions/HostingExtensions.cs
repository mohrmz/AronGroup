using AronGroup.Infrastructures.Data.Repositories;
using AronGroup.Infrastructures.Data.Repositories.Abstractions;
using AronGroup.Infrastructures.Data.Seeds;
using AronGroup.Infrastructures.Data.Seeds.Abstractions;
using AronGroup.Middlewares;
using AronGroup.Models.Common;
using Autofac.Core;

namespace AronGroup.Extensions;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        IConfiguration configuration = builder.Configuration;
        builder.Services.Configure<SiteSettings>(configuration.GetSection(nameof(SiteSettings)));
        var siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();

        builder.Services.AddRazorPages();
        builder.Services.AddControllersWithViews();
        builder.Services.AddHttpContextAccessor();
        
        builder.Services.AddDbContext(configuration);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddIdentity(siteSetting!.IdentitySettings);

        builder.Services.AddSwagger(configuration, "Swagger");

        builder.Services.ApiAuthentication();
        builder.Services.AddAuthorization();

        builder.Services.AddScoped<IDataInitializer, UserDataInitializer>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddSession(options =>
        {
            options.Cookie.IsEssential = true;
        });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.IntializeDatabase();

        app.UseSwaggerAndUI("Swagger");
        app.UseCookiePolicy();
        app.UseSession();

        app.UseStaticFiles();
        app.UseStatusCodePages();

        app.UseHttpsRedirection();

        app.UseSetApiKey();
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapDefaultControllerRoute();
        app.MapRazorPages();

        return app;
    }
}
