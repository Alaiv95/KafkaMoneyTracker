using Application.exceptions;
using System.Net;
using System.Text.Json;

namespace WebApi.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(ex, context);
        }
    }

    private Task HandleException (Exception ex, HttpContext context)
    {
        int code = (int) HttpStatusCode.InternalServerError;

        if (ex is IApiException exception)
        {
            code = exception.ErrorCode;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;

        var result = JsonSerializer.Serialize(new { error = ex.Message });

        return context.Response.WriteAsync(result);
    }
}
