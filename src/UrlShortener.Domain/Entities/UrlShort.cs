using System;

namespace UrlShortener.Domain.Entities
{
    public class UrlShort : IBaseEntity
    {
        public Guid Id { get; set; }
        public string FullUrl { get; set; }
        public string ShortenedUrl { get; set; }
    }
}