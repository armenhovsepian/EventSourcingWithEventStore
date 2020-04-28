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
            dbContext.Products.AddRange(pNames.Select((name, index) => new Product(name, rnd.Next(10, 100))));
            dbContext.SaveChanges();
        }
    }
}
