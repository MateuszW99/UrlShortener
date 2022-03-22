using System.Collections.Generic;
using System.Threading.Tasks;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Interfaces
{
    public interface IUrlStoreService
    {
        Task<string> GetFullUrl(int id);
        Task<int> SaveShortenedUrl(string fullUrl);
        Task<List<UrlShort>> GetAllShortenedUrls();
        Task<int> GetNextPossibleId();
    }
}