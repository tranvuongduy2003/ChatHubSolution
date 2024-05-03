using Cassandra.Mapping;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatHubSolution.Data.Entities
{
    public class Conversation : BaseEntity<string>
    {
        public string UserOneId { get; set; }
        public string UserTwoId { get; set; }
        public string ConnectionId { get; set; }
        public string Status { get; set; }

        [NotMapped]
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

        public static Map<Conversation> GetConfig(string keyspace)
        {
            return new Map<Conversation>()
                .KeyspaceName(keyspace)
                .TableName("conversations")
                .PartitionKey(x => x.Id)
                .Column(x => x.Id, x => x.WithName("id"))
                .Column(x => x.ConnectionId, x => x.WithName("connectionId"))
                .Column(x => x.UserOneId, x => x.WithName("userOneId"))
                .Column(x => x.UserTwoId, x => x.WithName("userTwoId"))
                .Column(x => x.Status, x => x.WithName("status"));
        }
    }
}
