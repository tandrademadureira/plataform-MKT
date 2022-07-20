using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Api.Configuration;
using Shared.Api.Middleware;
using Plataform.MKT.Api.Configurations;
using System;

namespace Plataform.MKT.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// The swagger title.
        /// </summary>
        public static string SwaggerDocTitle => "Plataform.MKT.Api";

        /// <summary>
        /// the swagger version.
        /// </summary>
        public static string SwaggerDocVersion => "v1";

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDataBase(Configuration);
            services.ConfigureDI();
            services.AddControllers(options => options.ConfigureMvcOptions());
            services.ConfigureAutorization(Configuration);
            services.ConfigureSwagger<Startup>(SwaggerDocTitle, SwaggerDocVersion);
            var assembly = AppDomain.CurrentDomain.Load("Plataform.MKT.Application");
            services.AddMediatR(assembly);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerConfig(SwaggerDocTitle, SwaggerDocVersion);
            }
            app.UseGlobalization(Configuration);
            app.UseMiddleware<LoggingMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
