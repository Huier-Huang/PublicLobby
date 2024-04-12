using System.Globalization;
using Serilog;

namespace PublicLobby_Server;

public static class Program
{
    public static void Main(string[] args)
    {
        var time = DateTime.Now.Date.ToString(CultureInfo.CurrentCulture);
        Log.Logger = new LoggerConfiguration().WriteTo.Console().WriteTo.File($"./logs/OutLog-{time}.log").CreateBootstrapLogger();
        
        var builder = CreateBuilder(args);
        
        try
        {
            Log.Information("Starting PublicLobby-Server v");
            builder.Build().Run();
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Not Start");
            throw;
        }
        
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IHostBuilder CreateBuilder(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        builder.UseContentRoot(Directory.GetCurrentDirectory());
        builder.UseSerilog();
        builder.ConfigureWebHostDefaults(hostBuilder =>
        {
            builder.ConfigureServices(collection =>
            {
                collection.AddControllers();
            });
        });
        return builder;
    }
}