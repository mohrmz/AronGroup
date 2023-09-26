using AronGroup.Infrastructures.Data.DbContext;
using AronGroup.Infrastructures.Data.Repositories.Abstractions;
using AronGroup.Models.Common;
using AronGroup.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AronGroup.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
        });

        return services;
    }

    public static IServiceCollection ApiAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options => options.DefaultScheme = ApiAuthenticationOptions.DefaultScheme)
                .AddScheme<ApiAuthenticationOptions, ApiAuthenticationHandler>(ApiAuthenticationOptions.DefaultScheme,
                                                                               options => { })
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Home/Index";
                    options.Cookie.IsEssential = true;
                    options.SlidingExpiration = true; 
                    options.ExpireTimeSpan = TimeSpan.FromSeconds(10);
                   
                });

        return services;
    }
}

public class ApiAuthenticationHandler : AuthenticationHandler<ApiAuthenticationOptions>
{
    private readonly IUserRepository _repository;
    private readonly IHttpContextAccessor _contextAccessor;
    public ApiAuthenticationHandler(IOptionsMonitor<ApiAuthenticationOptions> options,
                                    ILoggerFactory logger,
                                    UrlEncoder encoder,
                                    ISystemClock clock,
                                    IUserRepository repository,
                                    IHttpContextAccessor contextAccessor) : base(options, logger, encoder, clock)
    {
        _repository = repository;
        _contextAccessor = contextAccessor;
    }
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var a = _contextAccessor.HttpContext.Request.Headers.ToString();
        if ( !Request.Headers.ContainsKey(Options.TokenHeaderName))
        {
            return AuthenticateResult.Fail($"Missing header: {Options.TokenHeaderName}");
        }

        var option = new ApiAuthenticationOptions();
        string token = Request.Headers[Options.TokenHeaderName]!.ToString().Remove(ApiAuthenticationOptions.DefaultScheme, 1).Trim();
       
        var user = await _repository.GetByTokenAsync(token, _contextAccessor!.HttpContext!.RequestAborted);

        if (user is null)
        {
            return AuthenticateResult.Fail($"Invalid token.");
        }

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user!.UserName!),
            new Claim(ClaimTypes.Name, user!.FullName!),
            new Claim("Token", user!.Token!.ToString())
        };

        var claimsIdentity = new ClaimsIdentity
            (claims, this.Scheme.Name);
        var claimsPrincipal = new ClaimsPrincipal
            (claimsIdentity);

        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal,
                                                                  this.Scheme.Name));
    }
}
