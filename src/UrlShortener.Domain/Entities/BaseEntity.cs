using System;

namespace UrlShortener.Domain.Entities
{
    public interface IBaseEntity
    {
        public Guid Id { get; set; }
    }
}