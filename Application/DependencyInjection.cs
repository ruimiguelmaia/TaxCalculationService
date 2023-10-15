using Application.Services;
using Application.Validations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // MediatR
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // CalculationService
        services.AddTransient<ITaxCalculationService, TaxCalculationService>();

        return services;
    }
}