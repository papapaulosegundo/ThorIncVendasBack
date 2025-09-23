using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

using ThorAPI.Utils;

public class Auth {
    private readonly RequestDelegate _next;

    private readonly string[] _protectedPaths = new[] {
        "/api/usuario" 
    };

    public Auth(RequestDelegate next) {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context) {
        var path = context.Request.Path.Value?.ToLower();

        if (_protectedPaths.Any(p => path != null && path.StartsWith(p.ToLower()))) {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            var token = authHeader?.StartsWith("Bearer ") == true
                ? authHeader.Substring("Bearer ".Length).Trim()
                : null;

            if (string.IsNullOrEmpty(token)) {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Missing token");
                return;
            }

            var principal = Jwt.ValidateToken(token);
            if (principal == null) {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid or expired token");
                return;
            }

            context.Items["Usuario"] = principal; 
        }

        await _next(context);
    }
}

