using EventStore.ClientAPI;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI
{
    /// <summary>
    /// The connection to Event Store needs to open when our application starts and close when we shut down the application.
    /// </summary>
    public class HostedService : IHostedService
    {
        private readonly IEventStoreConnection _esConnection;
        public HostedService(IEventStoreConnection esConnection)
        {
            _esConnection = esConnection;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        => _esConnection.ConnectAsync();
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _esConnection.Close();
            return Task.CompletedTask;
        }
    }
}
