using ChatHubSolution.Extentions;
using ChatHubSolution.Helpers;
using EventHubSolution.BackendServer.Extentions;
using Serilog;

var AppCors = "AppCors";

var builder = WebApplication.CreateBuilder(args);

Log.Information("Starting ChatHub API up");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);

    builder.Host.AddAppConfigurations();

    builder.Services.AddInfrastructure(builder.Configuration, AppCors);

    builder.AddAppAuthetication();

    var app = builder.Build();

    app.UseInfrastructure(AppCors);

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information("Shut down ChatHub API complete");
    Log.CloseAndFlush();
}