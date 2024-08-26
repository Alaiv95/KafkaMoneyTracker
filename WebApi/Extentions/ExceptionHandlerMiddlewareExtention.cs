using WebApi.Middlewares;

namespace WebApi.Extentions;

public static class ExceptionHandlerMiddlewareExtention
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}
