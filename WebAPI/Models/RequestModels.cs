namespace WebAPI.Models
{
    public static class RequestModels
    {
        public static class V1
        {
            public class UpdateProductName
            {
                public int Id { get; set; }
                public string Name { get; set; }
            }

            public class UpdateProductPrice
            {
                public int Id { get; set; }
                public decimal Price { get; set; }
            }
        }
    }
}
