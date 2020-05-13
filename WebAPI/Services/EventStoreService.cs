using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Entities;
using WebAPI.Events;

namespace WebAPI.Services
{
    public interface IEventStoreService 
    { 
        Task<bool> Exists(int productId);
        Task<Product> Load(int productId);
        Task Save(Product entity);
    }

    public class EventStoreService : IEventStoreService
    {
        private readonly IEventStoreConnection _connection;
        public EventStoreService(IEventStoreConnection connection)
        {
            _connection = connection;
        }

        public async Task Save(Product entity)
        {

            var changes = entity.GetChanges()
                .Select(e => new EventData(
                    eventId: Guid.NewGuid(),
                    type: e.GetType().Name,
                    isJson: true,
                    data: Serialize(e),
                    metadata: Serialize(new EventMetadata { ClrType = e.GetType().AssemblyQualifiedName })));

            if (!changes.Any()) return;

            var streamName = GetStreamName(entity);

            await _connection.AppendToStreamAsync(
                streamName,
                ExpectedVersion.Any,
                changes);

            entity.ClearChanges();
        }


        public async Task<Product> Load(int productId)
        {
            var stream = GetStreamName(productId);
            var product = (Product)Activator.CreateInstance(typeof(Product), true);
            var page = await _connection.ReadStreamEventsForwardAsync(stream, 0, 1024, false);
            var events = page.Events.Select(resolvedEvent =>
            {
                var meta = JsonConvert.DeserializeObject<EventMetadata>(Encoding.UTF8.GetString(resolvedEvent.Event.Metadata));
                var dataType = Type.GetType(meta.ClrType);
                var createdData = resolvedEvent.Event.Created;
                var jsonData = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
                var data = JsonConvert.DeserializeObject(jsonData, dataType);
                return (IDomainEvent)data;
            });

            product.LoadChanges(events);
            return product;
        }


        public async Task<bool> Exists(int productId)
        {
            var stream = GetStreamName(productId);
            var result = await _connection.ReadEventAsync(stream, 1, false);
            return result.Status != EventReadStatus.NoStream;
        }


        private static byte[] Serialize(object data)
            => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

        private static string GetStreamName(int id)
            => $"{typeof(Product).Name}-{id.ToString()}";

        private static string GetStreamName(Product entity)
            => $"{typeof(Product).Name}-{entity.Id.ToString()}";


    }
}
