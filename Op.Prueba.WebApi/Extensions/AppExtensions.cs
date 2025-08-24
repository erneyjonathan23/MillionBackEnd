using OP.Prueba.WebAPI.Middleware;

namespace OP.Prueba.WebAPI.Extensions
{
    public static class AppExtensions
    {
        public static void UseErrorHandleMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
