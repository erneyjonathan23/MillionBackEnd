using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OP.Prueba.Application.Behaviours;
using System.Reflection;

namespace OP.Prueba.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            #region Services
            #endregion
        }
    }
}