using Application.mediator;
using Infrastructure;
using Infrastructure.Repositories;
using WebApi.Extentions;

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

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPersistence(configuration)
            .AddMediator();
        services.AddControllers();
        services.AddSwaggerGen();
    }

    private static void ConfigureApp(WebApplication app, IWebHostEnvironment env)
    {
        app.UseCustomExceptionHandler();
        app.UseSwagger();
        app.UseSwaggerUI(C =>
        {
            C.RoutePrefix = String.Empty;
            C.SwaggerEndpoint("swagger/v1/swagger.json", "Money Tracker Api");
        });
        app.UseRouting();
       
        app.UseEndpoints(endpoints => endpoints.MapControllers());
        app.Run();
    }
}