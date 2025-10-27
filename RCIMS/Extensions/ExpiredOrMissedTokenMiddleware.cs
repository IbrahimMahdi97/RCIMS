using Entities.ErrorModels;
using Microsoft.AspNetCore.Authorization;

namespace RCIMS.Extensions;

public class ExpiredOrMissedTokenMiddleware
{
    private readonly RequestDelegate _next;

    public ExpiredOrMissedTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is not null)
        {
            await _next(context);
            return;
        }
        if (context.User.Identity is not { IsAuthenticated: true })
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = 401,
                Message    = "The provided token is expired or missing !"
            }.ToString());
            return;
        }
        await _next(context);
    }
}