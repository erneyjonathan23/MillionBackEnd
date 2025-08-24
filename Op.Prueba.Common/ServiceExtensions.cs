using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OP.Prueba.Application.Interfaces;
using OP.Prueba.Common.Services;

namespace OP.Prueba.Common
{
    public static class ServiceExtensions
    {
        public static void AddSharedInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTimeService, DateTimeService>();
        }
    }
}
