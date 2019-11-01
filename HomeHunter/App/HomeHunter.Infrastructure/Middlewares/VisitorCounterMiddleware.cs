using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace HomeHunter.Infrastructure.Middlewares
{
    public class VisitorCounterMiddleware
    {
        private readonly RequestDelegate requestDelegate;

        public VisitorCounterMiddleware(RequestDelegate requestDelegate
            )
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            string visitorId = context.Request.Cookies["ai_user"];
            if (visitorId == null)
            {
                var newVisitorId = Guid.NewGuid().ToString();

                context.Response.Cookies.Append("ai_user", newVisitorId, new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(8),
                }); 
            }
            await this.requestDelegate(context);
        }
    }
}
