using Cassandra;
using Cassandra.Mapping;
using ChatHubSolution.Implementation;
using ChatHubSolution.Implementation.Interfaces;
using ChatHubSolution.Models;
using ChatHubSolution.Services;
using Microsoft.Extensions.Options;

namespace ChatHubSolution.Extensions
{
    public static class CassandraExtensions
    {
        public static IServiceCollection RegisterCassandra(this IServiceCollection services)
        {
            var scope = services.BuildServiceProvider();
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var cassandraOptions = scope.GetRequiredService<CassandraOptions>();
            var cassandraConfig = config.GetSection("Cassandra");
            services.Configure<CassandraConfig>(cassandraConfig);

            services.AddSingleton<ICluster>(provider =>
            {
                var conf = provider.GetRequiredService<IOptions<CassandraConfig>>();
                var queryOptions = new QueryOptions().SetConsistencyLevel(ConsistencyLevel.One);

                return Cluster.Builder()
                    .AddContactPoint(conf.Value.Host)
                    .WithPort(conf.Value.Port)
                    .WithCompression(CompressionType.LZ4)
                    .WithCredentials(conf.Value.Username, conf.Value.Password)
                    .WithQueryOptions(queryOptions)
                    .WithRetryPolicy(new LoggingRetryPolicy(new DefaultRetryPolicy()))
                    .Build();
            });
            services.AddScoped<ICassandraProvider, CassandraProvider>();

            // NOTE: Alternative way of mapping entity configurations
            // MappingConfiguration.Global.Define<CassandraMappings>();

            MappingConfiguration.Global.Define(cassandraOptions.Config);
            services.AddHostedService<CassandraHostedService>();

            return services;
        }
    }
}
