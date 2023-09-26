using AronGroup.Infrastructures.Data.DbContext;
using AronGroup.Models.Common;
using AronGroup.Models.User;

namespace AronGroup.Extensions;

public static class IdentityExtensions
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IdentitySettings settings)
    {
        services.AddIdentity<User, Role>(identityOptions =>
        {
            identityOptions.SignIn.RequireConfirmedAccount = false;
            identityOptions.Password.RequireDigit = settings.PasswordRequireDigit;
            identityOptions.Password.RequiredLength = settings.PasswordRequiredLength;
            identityOptions.Password.RequireNonAlphanumeric = settings.PasswordRequireNonAlphanumic;
            identityOptions.Password.RequireUppercase = settings.PasswordRequireUppercase;
            identityOptions.Password.RequireLowercase = settings.PasswordRequireLowercase;
            identityOptions.User.RequireUniqueEmail = settings.RequireUniqueEmail;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }
}
