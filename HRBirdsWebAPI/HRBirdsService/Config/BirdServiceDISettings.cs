using Microsoft.Extensions.DependencyInjection;
using HRBirdServices.Interface;
using HRBirdService.Interface;

namespace HRBirdService.Config
{
    static public class BirdServiceDISettings
    {
        public static IServiceCollection AddDI(IServiceCollection services)
        {
            services.AddTransient<IHRBirdService, HRBirdServices.Implementation.HRBirdService>();
            services.AddTransient<IBirdsSubmissionService, BirdsSubmissionService>();
            return services;
        }
    }
}
