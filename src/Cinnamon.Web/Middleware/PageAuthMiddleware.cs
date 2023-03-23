using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Cinnamon.Web.Enums;
namespace Cinnamon.Web.Middleware
{
    public class PageAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public PageAuthMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context, ClaimsPrincipal claimsPrincipal)
        {
            bool isAdmin = claimsPrincipal.IsInRole(nameof(UserRole.Admin).ToLower());

            var adminPages = new List<string> { "/admin/users", "/admin/experiences", "/admin/refunds" };

            if (isAdmin)
            {
                //if (!adminPages.Select(a => a.ToLower()).Contains(context.Request.Path.ToString().ToLower()))
                //{
                //    context.Response.Redirect("/admin/users");
                //}
            }
            else
            {
                if (adminPages.Select(a => a.ToLower()).Contains(context.Request.Path.ToString().ToLower()))
                {
                    context.Response.Redirect("/explore");
                }
            }
            await _next(context);
        }
    }
}
