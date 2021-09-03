using Application.Interfaces;
using Application.Interfaces.CacheRepositories;
using Application.Interfaces.Contexts;
using Application.Interfaces.Repositories;
using Application.Interfaces.Shared;
using Infrastructure.CacheRepositories;
using Infrastructure.DbContexts;
using Infrastructure.Repositories;
using Infrastructure.Services;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            #region Repositories
            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddTransient<IDateTimeService, DateTimeService>();

            services.AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>));
			services.AddTransient<ICommunityCacheRepository, CommunityCacheRepository>();
			services.AddTransient<IDistrictCacheRepository, DistrictCacheRepository>();
            services.AddTransient<ICommunityRepository, CommunityRepository>();
            services.AddTransient<IDistrictRepository, DistrictRepository>();
            services.AddTransient<ILogRepository, LogRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            #endregion Repositories
        }
    }
}
