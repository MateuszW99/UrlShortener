using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<UrlShort> UrlShorts { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
    
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<UrlShort> UrlShorts { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UrlShort>()
                .HasKey(p => p.Id);
            
            modelBuilder.Entity<UrlShort>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
        }
    }
}