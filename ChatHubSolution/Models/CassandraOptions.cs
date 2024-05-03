using Cassandra.Mapping;

namespace ChatHubSolution.Models
{
    public class CassandraOptions
    {
        public string? Keyspace { get; set; }
        public ITypeDefinition[]? Config { get; set; }
    };

}
