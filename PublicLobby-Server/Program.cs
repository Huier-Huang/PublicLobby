using System.Net;
using PublicLobby_Server.Managers;
using Serilog;

namespace PublicLobby_Server;

// Form Impostor
public static class Program
{
    public static void Main(string[] args)
    {
        var time = DateTime.Now.Date.ToString("MM-dd-HH-mm");
        Log.Logger = new LoggerConfiguration().
            WriteTo.Console().
            WriteTo.File($"./logs/OutLog-{time}.log".Replace("-", "_"))
            .CreateBootstrapLogger();
        
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
        var builder = Host.CreateDefaultBuilder(args)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseSerilog()
            .UseConsoleLifetime();
        
        var configuration = CreateConfiguration(args);
        var config = configuration
            .GetSection(ServerConfig.Section)
            .Get<ServerConfig>() ?? new ServerConfig();

        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            configurationBuilder.AddConfiguration(configuration);
        });
        
        builder.ConfigureServices((host, service) =>
        {
            service.AddSingleton<RoomManager>();
            service.AddSingleton<PlayerManager>();
            service.Configure<ServerConfig>(host.Configuration.GetSection(ServerConfig.Section));
        });
        
        builder.ConfigureWebHostDefaults(hostBuilder =>
        {
            
            builder.ConfigureServices(collection =>
            {
                collection.AddControllers();
                collection.AddEndpointsApiExplorer();
#if DEBUG
                collection.AddSwaggerGen();       
#endif
            });

            hostBuilder.Configure(appBuilder =>
            {
                appBuilder.UseRouting();
                appBuilder.UseEndpoints(endpoint =>
                {
                    endpoint.MapControllers();
#if DEBUG
                    endpoint.MapSwagger();
#endif
                });
#if DEBUG
                appBuilder.UseSwagger();
                appBuilder.UseSwaggerUI();
#endif
            });
            
            hostBuilder.ConfigureKestrel(kestrel =>
            {
#if DEBUG
                var address = IPAddress.Any;
                var endpoint = new IPEndPoint(address, 5032);
                kestrel.Listen(endpoint);
#else
                var address = IPAddress.Parse(config.ListenIp);
                var endpoint = new IPEndPoint(address, config.ListenPort);
                kestrel.Listen(endpoint);
#endif
            });
        });
        return builder;
    }
    
    private static IConfiguration CreateConfiguration(string[] args)
    {
        var configurationBuilder = new ConfigurationBuilder();

        configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
        configurationBuilder.AddJsonFile("config.json", true);
        configurationBuilder.AddCommandLine(args);

        return configurationBuilder.Build();
    }
}