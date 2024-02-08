using JobLink.DAL.Repositories.Implements;
using JobLink.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace JobLink.DAL;

public static class RepositoryRegistration
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IIndustryRepository, IndustryRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();
    }
}

