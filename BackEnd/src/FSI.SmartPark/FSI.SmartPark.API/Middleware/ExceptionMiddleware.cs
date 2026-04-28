using System.Net;
using System.Text.Json;

namespace FSI.SmartPark.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (KeyNotFoundException ex)
        {
            await Responder(ctx, HttpStatusCode.NotFound, ex.Message);
        }
        catch (ArgumentException ex)
        {
            await Responder(ctx, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await Responder(ctx, HttpStatusCode.Conflict, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            await Responder(ctx, HttpStatusCode.Unauthorized, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado");
            await Responder(ctx, HttpStatusCode.InternalServerError, "Erro interno do servidor.");
        }
    }

    private static async Task Responder(HttpContext ctx, HttpStatusCode status, string mensagem)
    {
        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = (int)status;
        var body = JsonSerializer.Serialize(new { erro = mensagem, status = (int)status });
        await ctx.Response.WriteAsync(body);
    }
}
