using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

public class SimpleMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public SimpleMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context)
    {
        // Get the token from the request headers
        var token = context.Request.Headers["Authorization"];

        if (string.IsNullOrEmpty(token))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Bạn chưa được xác thực");
            return;
        }

        try
        {
            // Validate the token using your secret key
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!)
                )
            };

            // Extract and validate the token
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

            // Check if the user is an admin
            if (principal.HasClaim(c => c.Type == "isAdmin"))
            {
                await _next(context); // User is an admin, continue with the request
            }
            else
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Bạn không có quyền thực hiện yêu cầu này");
            }
        }
        catch (Exception)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Token không hợp lệ");
        }
    }
}
