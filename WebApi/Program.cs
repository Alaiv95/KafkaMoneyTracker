using Application;
using Application.kafka;
using Application.MailClient;
using Infrastructure;
using Infrastructure.authentication;
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
        var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
        var redisName = Environment.GetEnvironmentVariable("REDIS_NAME");
        
        services.AddStackExchangeRedisCache(options => {
            options.Configuration = redisHost ?? "localhost";
            options.InstanceName = redisName ?? "local";
        });
        
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        services.Configure<KafkaOptions>(configuration.GetSection(nameof(KafkaOptions)));
        services.Configure<MailOptions>(configuration.GetSection(nameof(MailOptions)));

        services.PostConfigure<KafkaOptions>(options =>
        {
            var bootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS");
            if (!string.IsNullOrEmpty(bootstrapServers))
            {
                options.BootstrapServers = bootstrapServers;
            }
        });
        
        services.AddBearerApiAuth(configuration);
        services.AddControllers();
        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            options.IncludeXmlComments(xmlPath);
        });
        services
            .AddInfrastructure()
            .AddPersistence(configuration)
            .AddApplication();
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
        app.UseAuthentication();
        app.UseAuthorization();
       
        app.UseEndpoints(endpoints => endpoints.MapControllers());
        app.Run();
    }
}