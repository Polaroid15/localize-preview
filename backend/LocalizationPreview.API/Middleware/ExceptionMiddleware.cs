using System.Net.Mime;
using System.Text.Json;

namespace LocalizationPreview.API.Middleware; 

public class ExceptionMiddleware 
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);        
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        var resultObject = JsonSerializer.Serialize(new {
            ExceptionType = exception.GetType().FullName,
            Message = exception.Message,
            StatusCode = context.Response.StatusCode
        });
        await context.Response.WriteAsync(resultObject);
    }
}