using ChatServerApi.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using ChatServerApi.Helpers;
using System;
using System.IO;

namespace ChatServerApi
{
    public class Startup
    {
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;
        [Obsolete]
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            string projectRootPath = _hostingEnvironment.ContentRootPath;
            //LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            LogManager.LoadConfiguration(String.Concat(projectRootPath, "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        [Obsolete]
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors();
            //services.AddControllers();


            services.ConfigureCors();

            services.ConfigureIISIntegration();

            services.ConfigureLoggerService();

            //services.ConfigureMySqlContext(Configuration);

            //services.ConfigureRepositoryWrapper();
            services.AddControllers();

            services.AddSignalR().AddMessagePackProtocol();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddHttpContextAccessor();
            services.AddAuthorization();

            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();
        }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		[Obsolete]
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			//else
			//{
			//	app.Use(async (context, next) =>
			//	{
			//		await next();
			//		if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
			//		{
			//			context.Request.Path = "/";
			//			await next();
			//		}
			//	});
			//	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			//	//app.UseHsts();
			//}

			app.UseHttpsRedirection();

			//app.UseCors("CorsPolicy");

			//app.UseForwardedHeaders(new ForwardedHeadersOptions
			//{
			//    ForwardedHeaders = ForwardedHeaders.All
			//});

			//app.UseStaticFiles();

			app.UseRouting();


			////app.UseHttpsRedirection();


			app.UseStaticFiles();
			

			//app.UseMvc();
			app.UseSession();


            app.UseCors();


            //Add JWToken to all incoming HTTP Request Header
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });
            //Add JWToken Authentication service
            app.UseAuthentication();
            //app.UseCookiePolicy();
            //app.UseAuthorization();


            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/signalr");
            });
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Welcome to Bangladesh Software Ltd. Chat Api.");
            });
        }
    }
}
