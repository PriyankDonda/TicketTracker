using System.Net;
using TicketTracker.Helpers;

namespace TicketTracker.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Exception occured at {context.Request.Path}");
            
            var data = new
            {
                Exception = e.Message,
                StackTrace = e.StackTrace
            };
                
            ApiResponseHelper.Error<object>(data, e.Message, HttpStatusCode.InternalServerError);
        }
    }
}