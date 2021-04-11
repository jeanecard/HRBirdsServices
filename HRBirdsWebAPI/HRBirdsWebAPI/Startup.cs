using HRBirdRepository;
using HRBirdsEntities;
using HRBirdService.Config;
using HRBirdServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace HRBirdsWebAPI
{
    public class Startup
    {
        private static readonly String _VERSION_FOR_SWAGGER_DISLPAY = "Version 1";
        private static readonly String _NAME_FOR_SWAGGER_DISLPAY = "HR Birds Services";
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add functionality to inject IOptions<T>
            services.AddOptions();
            // Add our Config object so it can be injected
            services.Configure<HRAzureBlobConfig>(Configuration.GetSection("HRAzureBlob"));

            //Add azure signalR Reference
            services.AddSignalR(); 

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed((hosts) => true)); //allow anything
            });
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

            app.UseCors("CorsPolicy");
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseFileServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<HRBirdPictureSubmissionHub>("/HRBirdPictureSubmission");
            });
        }
    }
}
