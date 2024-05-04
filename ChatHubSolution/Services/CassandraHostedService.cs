using Cassandra;
using Cassandra.Data.Linq;
using ChatHubSolution.Data.Entities;
using ChatHubSolution.Implementation.Interfaces;
using ChatHubSolution.Models;

namespace ChatHubSolution.Services
{
    public class CassandraHostedService(ILogger<CassandraHostedService> logger, ICluster cluster, IServiceScopeFactory factory, CassandraOptions options) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var scope = factory.CreateScope().ServiceProvider;
            var bootSession = cluster.Connect();
            var cassandraProvider = scope.GetRequiredService<ICassandraProvider>();

            bootSession.CreateKeyspaceIfNotExists(options.Keyspace);
            logger.LogInformation("Creating cassandra default keyspace");

            try
            {
                var session = cassandraProvider.GetSession();

                await new Table<User>(session).CreateIfNotExistsAsync();
                await new Table<Message>(session).CreateIfNotExistsAsync();
                await new Table<Conversation>(session).CreateIfNotExistsAsync();
            }
            catch (Exception)
            {
                logger.LogInformation("Tables creation failed");
                throw;
            }

            var keyspaces = new List<string>(cluster.Metadata.GetKeyspaces());
            keyspaces.ForEach((value) =>
            {
                if (value == options.Keyspace)
                {
                    logger.LogInformation("KeySpace: " + value);
                    new List<string>(cluster.Metadata.GetKeyspace(value).GetTablesNames()).ForEach((tableName) =>
                    {
                        Console.WriteLine("Table: " + tableName);
                    });
                }
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Finishing cassandra background activities");
            return Task.CompletedTask;
        }
    }
}
