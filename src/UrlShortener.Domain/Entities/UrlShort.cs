using System;

namespace UrlShortener.Domain.Entities
{
    public class UrlShort : IBaseEntity
    {
        public int Id { get; set; }
        public string FullUrl { get; set; }
    }
}