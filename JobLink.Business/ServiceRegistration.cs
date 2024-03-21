using JobLink.Business.ExternalServices.Implements;
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
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IIndustryService, IndustryService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAdvertisementService, AdvertisementService>();
        services.AddScoped<IAbilityService, AbilityService>();
        services.AddScoped<IJobDescriptionService, JobDescriptionService>();
    }
} 

