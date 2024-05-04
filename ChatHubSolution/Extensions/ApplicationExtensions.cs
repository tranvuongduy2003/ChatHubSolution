using ChatHubSolution.Extensions;
using ChatHubSolution.Hubs;

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
        }
    }
}
