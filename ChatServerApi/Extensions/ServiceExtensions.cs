using Interfaces;
using Entities;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChatServerApi;

namespace ChatServerApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
			services.AddCors(options =>
			{
				options.AddDefaultPolicy(builder =>
				{
					builder
						.WithOrigins(
						"http://localhost")
						.AllowCredentials()
						.AllowAnyHeader()
						.SetIsOriginAllowed(_ => true)
						.AllowAnyMethod();
				});
			});
			//services.AddCors(options =>
			//{
			//    options.AddPolicy("CorsPolicy",
			//        builder => builder.AllowAnyOrigin()
			//        .AllowAnyMethod()
			//        .AllowAnyHeader()
			//        .AllowCredentials());
			//});
		}

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options => 
            {
                
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddScoped<ILoggerManager, LoggerManager>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
        }

        //public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration config)
        //{
        //    var connectionString = config["mysqlconnection:connectionString"];
        //    services.AddDbContext<RepositoryContext>(o => o.UseMySql(connectionString));
        //}

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            //services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
    }
}
