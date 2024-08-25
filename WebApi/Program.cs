using Application.mediator;
using Infrastructure;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<MoneyTrackerDbContext>();
                context.Database.EnsureCreated();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        var env = builder.Environment;
        ConfigureApp(app, env);
    }

    private static void ConfigureServices(IServiceCollection collection, IConfiguration configuration)
    {
        collection
            .AddPersistence(configuration)
            .AddMediator();
    }

    private static void ConfigureApp(WebApplication app, IWebHostEnvironment env)
    {
        app.MapGet("/", () => "Hello World!");
        app.Run();
    }
}