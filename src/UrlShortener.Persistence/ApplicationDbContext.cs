using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<UrlShort> UrlShorts { get; set; }
    }
    
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<UrlShort> UrlShorts { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}