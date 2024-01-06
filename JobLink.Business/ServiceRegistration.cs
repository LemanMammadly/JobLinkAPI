﻿using JobLink.Business.ExternalServices.Implements;
using JobLink.Business.ExternalServices.Interfaces;
using JobLink.Business.Services.Implements;
using JobLink.Business.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace JobLink.Business;

public static class ServiceRegistration
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAppUserService, AppUserService>();
        services.AddScoped<ITokenService, TokenService>();
    }
} 

