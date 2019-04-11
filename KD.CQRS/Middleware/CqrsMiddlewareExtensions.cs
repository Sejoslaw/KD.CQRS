using KD.CQRS.Middleware;
using KD.CQRS.Providers;
using Microsoft.AspNetCore.Http;
using System;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Contains all static methods for adding CQRS middleware.
    /// </summary>
    public static class CqrsMiddlewareExtensions
    {
        public static IApplicationBuilder UseCqrs(this IApplicationBuilder builder, ICqrsProvider cqrsProvider, Func<HttpContext, bool> authorizeFunc)
        {
            return builder.UseMiddleware<CqrsMiddleware>(authorizeFunc, cqrsProvider, builder.ApplicationServices);
        }
    }
}
