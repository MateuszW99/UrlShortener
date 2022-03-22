using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entities;
using UrlShortener.Persistence;

namespace UrlShortener.Application.Services
{
    public class UrlStoreService : IUrlStoreService
    {
        private readonly IApplicationDbContext _context;
        private readonly int _lowestPossibleId;
        
        public UrlStoreService(IApplicationDbContext context)
        {
            _context = context;
            _lowestPossibleId = 1234;
        }

        public async Task<string> GetFullUrl(int id)
        {
            var entity = await _context.UrlShorts.FirstOrDefaultAsync(x => x.Id == id);
            return entity?.FullUrl;
        }

        public async Task<int> SaveShortenedUrl(string fullUrl)
        {
            var nextId = await GetNextPossibleId();
            
            var urlShortEntity = new UrlShort()
            {
                Id = nextId,
                FullUrl = fullUrl
            };

            await _context.UrlShorts.AddAsync(urlShortEntity);
            await _context.SaveChangesAsync(CancellationToken.None);
            return await Task.FromResult(nextId);
        }

        public async Task<List<UrlShort>> GetAllShortenedUrls()
        {
            var urls = await _context.UrlShorts.ToListAsync();
            return urls;
        }

        public async Task<int> GetNextPossibleId()
        {
            var lastRecord = await _context.UrlShorts.LastOrDefaultAsync();

            if (lastRecord is null)
            {
                return await Task.FromResult(_lowestPossibleId);
            }

            return await Task.FromResult(lastRecord.Id + 1);
        }
    }
}