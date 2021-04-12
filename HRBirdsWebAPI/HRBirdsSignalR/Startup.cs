using HRBirdsSignalR.Interface;
using HRBirdsSignalR.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace HRBirdsSignalR
{
    public class Startup
    {
        private static readonly String _VERSION_FOR_SWAGGER_DISLPAY = "Version 1";
        private static readonly String _NAME_FOR_SWAGGER_DISLPAY = "HR Birds SignalR Notification";
        private static readonly String _CORS_POLICY = "CorsPolicy";
        public static readonly String SIGNALR_ENDPOINT = "/HRBirdPictureSubmission";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //DI
            services.AddTransient<IHRPictureSignalRService, HRPictureSignalRService>();
            //Swagger
            services.AddSwaggerDocument(swagger => {
                swagger.Version = _VERSION_FOR_SWAGGER_DISLPAY;
                swagger.Title = _NAME_FOR_SWAGGER_DISLPAY;
                swagger.GenerateEnumMappingDescription = true;
            });

            services.AddControllers();

            //Add azure signalR Reference
            services.AddSignalR();

            services.AddCors(options =>
            {
                options.AddPolicy(
                    _CORS_POLICY,
                    builder => builder
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
            app.UseCors(_CORS_POLICY);
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseFileServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<HRBirdPictureSubmissionHub>(SIGNALR_ENDPOINT);
            });
        }
    }
}
