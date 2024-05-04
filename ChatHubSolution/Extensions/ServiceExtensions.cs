using ChatHubSolution.Constants;
using ChatHubSolution.Data.Entities;
using ChatHubSolution.Extensions;
using ChatHubSolution.Models;
using ChatHubSolution.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace ChatHubSolution.Extentions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string appCors)
        {
            services.AddControllers();

            services.AddAppConfigures(configuration);

            services.ConfigureSwagger();

            services.AddSignalR();

            services.AddCors(p =>
                p.AddPolicy(appCors, build => { build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader(); }));

            services.AddControllers();
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddInfrastructureServices();

            services.AddSingleton(new CassandraOptions
            {
                Keyspace = CassandraConstant.Keyspace,
                Config = [
                    User.GetConfig(CassandraConstant.Keyspace),
                    Conversation.GetConfig(CassandraConstant.Keyspace),
                    Message.GetConfig(CassandraConstant.Keyspace),
                ]
            });
            services.RegisterCassandra();

            return services;
        }

        private static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization PageOrder as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[] { }
                    }
                });
            });

            return services;
        }

        private static IServiceCollection AddAppConfigures(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.Configure<JwtOptions>(configuration.GetSection("ApiSettings:JwtOptions"));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }

        private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();

            return services;
        }
    }
}
