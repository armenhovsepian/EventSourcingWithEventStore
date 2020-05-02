using Newtonsoft.Json;
using System;

namespace WebAPI.Events
{
    public static class Events
    {
        public class BaseProductEvent
        {
            public int Id { get; set; }

            [JsonIgnore]
            public DateTime Created { get; set; }

            [JsonIgnore]
            public virtual string SimpleTypeName => GetType().Name;
            [JsonIgnore]
            public virtual string FullyQuailifiedTypeName => GetType().AssemblyQualifiedName; // FQCN
        }

        public class ProductCreated
        {
            public int Id { get; set; }
            public int UserId { get; set; }
        }

        public class ProductNameUpdated
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class ProductPriceUpdated
        {
            public int Id { get; set; }
            public decimal Price { get; set; }
        }
    }
}
