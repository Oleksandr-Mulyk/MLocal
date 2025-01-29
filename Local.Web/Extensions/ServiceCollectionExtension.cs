using Local.Web.Components.Account;
using Local.Web.Data;
using Microsoft.AspNetCore.Components.Authorization;

namespace Local.Web.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddCascadingAuthenticationState();
            services.AddScoped<IdentityUserAccessor>();
            services.AddScoped<IdentityRedirectManager>();
            services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            services.AddTransient<IUserRepository, UserRepository>();

            return services;
        }
    }
}
