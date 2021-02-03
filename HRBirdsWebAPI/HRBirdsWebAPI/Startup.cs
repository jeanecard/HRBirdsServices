using HRBirdRepository;
using HRBirdService.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRBirdsWebAPI
{
    public class Startup
    {
        private static readonly String _VERSION_FOR_SWAGGER_DISLPAY = "Version 1";
        private static readonly String _NAME_FOR_SWAGGER_DISLPAY = "HR Birds Services";
        // private readonly string _ALLOW_SPECIFIC_ORIGIN = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //DI
            BirdServiceDISettings.AddDI(services);
            BirdRepositoryDISettings.AddDI(services);

            //Swagger
            services.AddSwaggerDocument(swagger => {
                swagger.Version = _VERSION_FOR_SWAGGER_DISLPAY;
                swagger.Title = _NAME_FOR_SWAGGER_DISLPAY;
                swagger.GenerateEnumMappingDescription = true;
            });
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
