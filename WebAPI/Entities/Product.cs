using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Events;
using static WebAPI.Events.ProductEvents;

namespace WebAPI.Entities
{
    public class Product
    {
        private readonly List<IDomainEvent> _events;
        public IEnumerable<IDomainEvent> GetChanges() => _events.AsEnumerable();
        protected Product() => _events = new List<IDomainEvent>();
        public Product(string name, decimal price) : this()
        {
            Name = name;
            Price = price;
            var evt = new ProductCreated
            {
                Id = Id            
            };
            _events.Add(evt);
        }

        public int Id { get; set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public DateTime Created { get; private set; } = DateTime.Now;
        public DateTime? Modified { get; private set; }


       

        public void UpdateName(string name)
        {
            Name = name;
            Modified = DateTime.Now;
            var evt = new ProductNameUpdated
            {
                Id = Id,
                Name = name
            };
            _events.Add(evt);
        }

        public void UpdatePrice(decimal price)
        {
            Price = price;
            Modified = DateTime.Now;
            var evt = new ProductPriceUpdated
            {
                Id = Id,
                Price = price
            };
            _events.Add(evt);
        }

        public void LoadChanges(IEnumerable<IDomainEvent> history)
        {
            foreach (var e in history)
                _events.Add(e);
            
        }

        public void ClearChanges() => _events.Clear();
    }
}
