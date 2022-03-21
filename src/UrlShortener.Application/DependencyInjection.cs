using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
    }
}