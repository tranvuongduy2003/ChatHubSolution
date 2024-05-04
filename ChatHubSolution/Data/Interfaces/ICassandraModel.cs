namespace ChatHubSolution.Data.Interfaces
{
    public interface ICassandraModel<TKey> where TKey : notnull
    {
        public TKey Id { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
