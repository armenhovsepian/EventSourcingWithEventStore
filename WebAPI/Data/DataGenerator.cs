using System;
using System.Linq;
using WebAPI.Entities;

namespace WebAPI.Data
{
    public static class DataGenerator
    {
        public static void Initialize(AppDbContext dbContext)
        {
            var rnd = new Random();
            var pNames = new string[] { "Beef", "Pork", "Wine", "Pasta", "Tea" };
            dbContext.Products.AddRange(pNames.Select((name, index) => new Product
            {
                Name = name,
                Price = rnd.Next(10, 100),
                Created = DateTime.Now
            }));
            dbContext.SaveChanges();
        }
    }
}
