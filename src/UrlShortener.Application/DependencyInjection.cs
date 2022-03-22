using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Services;
using UrlShortener.Persistence;

namespace UrlShortener.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInMemoryDbContext(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("UrlShortenerDB");
            });
            
            return services;
        }

        public static IServiceCollection InstallServices(this IServiceCollection services)
        {
            services.AddTransient<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddSingleton<IUrlShortener, UrlShortenerService>();
            services.AddTransient<IUrlStoreService, UrlStoreService>();

            return services;
        }
    }
}