namespace ChatHubSolution.Implementation.Interfaces
{
    public interface ICassandraProvider
    {
        Cassandra.ISession GetSession();

        Cassandra.ISession GetSession(string keyspace);
    }
}
