namespace WebAPI.Events
{
    public static class Events
    {
        public class ProductCreated
        {
            public int Id { get; set; }
            //public int UserId { get; set; }
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
