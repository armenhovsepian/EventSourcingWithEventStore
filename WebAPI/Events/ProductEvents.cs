using Newtonsoft.Json;
using System;

namespace WebAPI.Events
{
    public static class ProductEvents
    {
        public class BaseProductEvent : IDomainEvent
        {
            public int Id { get; set; }

            //[JsonIgnore]
            public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

            [JsonIgnore]
            public virtual string SimpleTypeName => GetType().Name;
            [JsonIgnore]
            public virtual string FullyQuailifiedTypeName => GetType().AssemblyQualifiedName; // FQCN
        }

        public class ProductCreated : BaseProductEvent
        {
            public int UserId { get; set; }
        }

        public class ProductNameUpdated : BaseProductEvent
        {
            public string Name { get; set; }
        }

        public class ProductPriceUpdated : BaseProductEvent
        {
            public decimal Price { get; set; }
        }
    }
}
