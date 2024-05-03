using Cassandra.Mapping;
using ChatHubSolution.Constants;
using ChatHubSolution.Data.Entities;

namespace ChatHubSolution
{
    public class CassandraMappings : Mappings
    {
        public const string keyspace = CanssandraConstant.Keyspace;
        public CassandraMappings()
        {
            For<User>()
                .KeyspaceName(keyspace)
                .TableName("users")
                .PartitionKey(x => x.Id)
                // .ClusteringKey(x => x.Id)
                .Column(x => x.Id, x => x.WithName("id"))
                .Column(x => x.Email, x => x.WithName("email"))
                .Column(x => x.Name, x => x.WithName("name"));
        }
    }
}
