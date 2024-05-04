using Cassandra.Mapping;

namespace ChatHubSolution.Data.Entities
{
    public class Conversation : BaseEntity<string>
    {
        public string UserOneId { get; set; }
        public string UserTwoId { get; set; }
        public string ConnectionId { get; set; }
        public string Status { get; set; }

        public static Map<Conversation> GetConfig(string keyspace)
        {
            return new Map<Conversation>()
                .KeyspaceName(keyspace)
                .TableName("conversations")
                .PartitionKey(x => x.Id)
                .Column(x => x.Id, x => x.WithName("id"))
                .Column(x => x.UserOneId, x => x.WithName("useroneid"))
                .Column(x => x.UserTwoId, x => x.WithName("usertwoid"))
                .Column(x => x.ConnectionId, x => x.WithName("connectionid"))
                .Column(x => x.Status, x => x.WithName("status"))
                .Column(x => x.CreatedAt, x => x.WithName("createdat"))
                .Column(x => x.UpdatedAt, x => x.WithName("updatedat"));
        }
    }
}
