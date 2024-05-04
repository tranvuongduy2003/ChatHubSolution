using Cassandra.Mapping;

namespace ChatHubSolution.Data.Entities
{
    public class User : BaseEntity<string>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public static Map<User> GetConfig(string keyspace)
        {
            return new Map<User>()
                .KeyspaceName(keyspace)
                .TableName("users")
                .PartitionKey(x => x.Id)
                .Column(x => x.Id, x => x.WithName("id"))
                .Column(x => x.Name, x => x.WithName("name"))
                .Column(x => x.Email, x => x.WithName("email"))
                .Column(x => x.Password, x => x.WithName("password"))
                .Column(x => x.CreatedAt, x => x.WithName("createdat"))
                .Column(x => x.UpdatedAt, x => x.WithName("updatedat"));
        }
    }
}
