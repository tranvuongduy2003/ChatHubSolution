namespace ChatHubSolution.Data.Interfaces
{
    public interface ICassandraModel<TKey> where TKey : notnull
    {
        public TKey Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
