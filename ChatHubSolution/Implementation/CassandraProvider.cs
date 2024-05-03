using Cassandra;
using ChatHubSolution.Implementation.Interfaces;
using ChatHubSolution.Models;

namespace ChatHubSolution.Implementation
{

    public class CassandraProvider : ICassandraProvider, IAsyncDisposable
    {
        private readonly ILogger<CassandraProvider> _logger;
        private readonly ICluster _cluster;
        private readonly Cassandra.ISession _session;

        public CassandraProvider(ILogger<CassandraProvider> logger, ICluster cluster, CassandraOptions options)
        {
            _logger = logger;
            _cluster = cluster;
            _session = _cluster.Connect(options.Keyspace);
        }

        public Cassandra.ISession GetSession() => _session;

        public Cassandra.ISession GetSession(string keyspace)
        {
            try
            {
                _cluster.Connect(keyspace);
            }
            catch (Exception)
            {
                _logger.LogCritical("Error connecting to cassandra session");
                throw;
            }

            return _session;
        }

        public async ValueTask DisposeAsync()
        {
            await _session.ShutdownAsync();
        }
    }
}
