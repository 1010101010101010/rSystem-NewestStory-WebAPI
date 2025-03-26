using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Application.Adapters;
using Infrastructure.Repositories;
using Domain.Interfaces;
using Application.Interfaces;
using Application.Services;

namespace Application
{
    public class ServiceRegister
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register services here
            services.AddDbContext<rSystemContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MyDbConnection"));
            });
            // Register repositories here
            services.AddScoped<IStoryRepository, StoryRepository>();
            // Register adapters here
            services.AddScoped<IBaseAdapater,ExternalApiAdapter>();
            // Register services here
            services.AddSingleton<ICachingService, CachingService>();
            services.AddScoped<IStoryServices,StoryServices>();
        }
    }
}
