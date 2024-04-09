using System;
using api.Foundation.Services.Patients;
using api.Repositories;
using api.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<TestTaskDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));
        services.AddScoped<IPatientsRepository, PatientsRepository>();
        services.AddScoped<IPatientsService, PatientsService>();

        return services;
    }
}