using ChatHubSolution.Data;
using ChatHubSolution.Extensions;
using ChatHubSolution.Hubs;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ChatHubSolution.Extentions
{
    public static class ApplicationExtensions
    {
        public static void UseInfrastructure(this WebApplication app, string appCors)
        {
            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat Hub API V1");
            });

            //app.UseHttpsRedirection(); //production only

            app.UseErrorWrapping();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapHub<ChatHub>("/Chat");

            app.UseCors(appCors);
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();
                var context = services.GetRequiredService<ApplicationDbContext>();

                try
                {
                    logger.LogInformation("Migrating database.");
                    if (context.Database.GetPendingMigrations().Count() > 0)
                        context.Database.Migrate();
                    logger.LogInformation("Migrated database.");
                    Log.Information("Seeding data...");
                    var dbInitializer = services.GetService<DbInitializer>();
                    dbInitializer?.Seed().Wait();
                    Log.Information("Seeding data successfully!");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}
