using Local.Web.Components.Account;
using Local.Web.Components.Layout.Alerts;
using Local.Web.Data;
using Local.Web.Data.ToDo;
using Local.Web.Data.User;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

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
            services.AddTransient<IRepository<IdentityRole, string>, RoleRepository>();
            services.AddTransient<IRepository<IToDoItem>, ToDoRepository>();

            services.AddSingleton<IMessageManager, MessageManager>();

            return services;
        }
    }
}
