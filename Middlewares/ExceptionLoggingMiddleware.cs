using CareerSim.Services.IServices;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

public class ExceptionLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IErrorLogService logService)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var userId = context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var path = context.Request.Path;
            var browser = context.Request.Headers["User-Agent"].ToString();

            await logService.LogAsync(ex, path, userId, browser);

            throw; // Uygulamanın default hata davranışı devam etsin
        }
    }
}
