using System;
using System.Collections.Generic;
using System.Linq;
using static WebAPI.Events.Events;

namespace WebAPI.Entities
{
    public class Product
    {
        private readonly List<object> _events;
        public IEnumerable<object> GetChanges() => _events.AsEnumerable();
        protected Product() => _events = new List<object>();
        public Product(string name, decimal price) : this()
        {
            Name = name;
            Price = price;
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

        public void LoadChanges(IEnumerable<object> history)
        {
            foreach (var e in history)
                _events.Add(e);
            
        }

        public void ClearChanges() => _events.Clear();
    }
}
