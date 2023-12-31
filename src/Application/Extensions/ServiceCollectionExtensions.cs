﻿using System.Reflection;
using Application.Features.Event.Commands;
using Application.Features.Event.Queries;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection service)
        {
            service.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetEventsByCategoryAsync)));
            service.AddAutoMapper(Assembly.GetExecutingAssembly());
            service.AddFluentValidationAutoValidation();
            service.AddValidatorsFromAssemblyContaining<CreateEventAsync>();
        }
    }
}
