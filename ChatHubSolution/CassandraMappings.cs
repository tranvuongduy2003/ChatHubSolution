using Cassandra.Mapping;
using ChatHubSolution.Constants;
using ChatHubSolution.Data.Entities;

namespace ChatHubSolution
{
    public class CassandraMappings : Mappings
    {
        public const string keyspace = CassandraConstant.Keyspace;
        public CassandraMappings()
        {
            For<User>()
                .KeyspaceName(keyspace)
                .TableName("users")
                .PartitionKey(x => x.Id)
                .Column(x => x.Id, x => x.WithName("id"))
                .Column(x => x.Name, x => x.WithName("name"))
                .Column(x => x.Email, x => x.WithName("email"))
                .Column(x => x.Password, x => x.WithName("password"))
                .Column(x => x.CreatedAt, x => x.WithName("createdat"))
                .Column(x => x.UpdatedAt, x => x.WithName("updatedat"));

            For<Conversation>()
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

            For<Message>()
                .KeyspaceName(keyspace)
                .TableName("messages")
                .PartitionKey(x => x.Id)
                .Column(x => x.Id, x => x.WithName("id"))
                .Column(x => x.UserId, x => x.WithName("userid"))
                .Column(x => x.ConversationId, x => x.WithName("conversationid"))
                .Column(x => x.Content, x => x.WithName("content"))
                .Column(x => x.Status, x => x.WithName("status"))
                .Column(x => x.CreatedAt, x => x.WithName("createdat"))
                .Column(x => x.UpdatedAt, x => x.WithName("updatedat"));
        }
    }
}
