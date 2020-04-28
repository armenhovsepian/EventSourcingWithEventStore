namespace WebAPI
{
    public class AppSettings
    {
        public EventStore EventStoreSetting { get; set; }


        public class EventStore
        {
            public string ConnectionString { get; set; }
        }
    }
}
