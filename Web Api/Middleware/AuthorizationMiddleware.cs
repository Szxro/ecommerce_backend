using Application.Common.Interfaces;

namespace Web_Api.Middleware
{
    public class AuthorizationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //Getting the token from the request
            string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token is not null)
            {
                var tokenServices = context.RequestServices.GetRequiredService<ITokenService>();

                var (roles,_) = tokenServices.ValidateAndReturnClaims(token);

                context.Items["roles"] = roles; // adding the roles to the current httpcontext
            }

            await next.Invoke(context);
        }
    }
}
