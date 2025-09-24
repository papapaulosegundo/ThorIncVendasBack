using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using ThorAPI.Utils;

public class Auth
{
    private readonly RequestDelegate _next;

    public Auth(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var path = (context.Request.Path.Value ?? string.Empty)
            .TrimEnd('/')
            .ToLowerInvariant();

        if (path == "/api/usuario/login" || path == "/api/usuario/signup")
        {
            await _next(context);
            return;
        }

        // Protege qualquer coisa sob /api/usuario
        if (path.StartsWith("/api/usuario"))
        {
            var header = context.Request.Headers["Authorization"].FirstOrDefault();
            var token = (header?.StartsWith("Bearer ", System.StringComparison.OrdinalIgnoreCase) ?? false)
                ? header.Substring("Bearer ".Length).Trim()
                : null;

            if (string.IsNullOrWhiteSpace(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Missing token");
                return;
            }

            var principal = Jwt.ValidateToken(token);
            if (principal is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid or expired token");
                return;
            }

            context.Items["Usuario"] = principal;
        }

        await _next(context);
    }
}
