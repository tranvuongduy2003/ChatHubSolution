using Cassandra.Mapping;

namespace ChatHubSolution.Data.Entities
{
    public class Message : BaseEntity<string>
    {
        public string UserId { get; set; }
        public string ConversationId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Status { get; set; }


        public static Map<Message> GetConfig(string keyspace)
        {
            return new Map<Message>()
                .KeyspaceName(keyspace)
                .TableName("messages")
                .PartitionKey(x => x.Id)
                .Column(x => x.Id, x => x.WithName("id"))
                .Column(x => x.UserId, x => x.WithName("userId"))
                .Column(x => x.ConversationId, x => x.WithName("conversationId"))
                .Column(x => x.Status, x => x.WithName("status"));
        }
    }
}
