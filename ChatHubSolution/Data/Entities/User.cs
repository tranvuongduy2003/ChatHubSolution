using Cassandra.Mapping;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatHubSolution.Data.Entities
{
    public class User : BaseEntity<string>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        [NotMapped]
        public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

        [NotMapped]
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

        public static Map<User> GetConfig(string keyspace)
        {
            return new Map<User>()
                .KeyspaceName(keyspace)
                .TableName("users")
                .PartitionKey(x => x.Id)
                .Column(x => x.Id, x => x.WithName("id"))
                .Column(x => x.Name, x => x.WithName("name"))
                .Column(x => x.Email, x => x.WithName("email"))
                .Column(x => x.Password, x => x.WithName("password"));
        }
    }
}
