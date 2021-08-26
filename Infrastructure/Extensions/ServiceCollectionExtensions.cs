using AdminPanel.Application.Interfaces;
using AdminPanel.Application.Interfaces.CacheRepositories;
using AdminPanel.Application.Interfaces.Contexts;
using AdminPanel.Application.Interfaces.Repositories;
using AdminPanel.Application.Interfaces.Shared;
using AdminPanel.Infrastructure.CacheRepositories;
using AdminPanel.Infrastructure.DbContexts;
using AdminPanel.Infrastructure.Repositories;
using AdminPanel.Infrastructure.Services;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AdminPanel.Infrastructure.Extensions
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
			services.AddTransient<ICommunityRepository, CommunityRepository>();
            services.AddTransient<ILogRepository, LogRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            #endregion Repositories
        }
    }
}
