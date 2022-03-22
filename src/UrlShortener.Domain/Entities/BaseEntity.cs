using System;

namespace UrlShortener.Domain.Entities
{
    public interface IBaseEntity
    {
        public int Id { get; set; }
    }
}