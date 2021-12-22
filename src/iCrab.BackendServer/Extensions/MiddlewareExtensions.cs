using iCrabee.BackendServer.Helpers;
using Microsoft.AspNetCore.Builder;

namespace iCrabee.BackendServer.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorWrapping(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorWrappingMiddleware>();
        }
    }
}
