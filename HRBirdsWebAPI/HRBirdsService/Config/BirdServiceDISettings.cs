using Microsoft.Extensions.DependencyInjection;
using HRBirdServices.Interface;
using HRBirdService.Interface;
using HRBirdService.Business;

namespace HRBirdService.Config
{
    static public class BirdServiceDISettings
    {
        public static IServiceCollection AddDI(IServiceCollection services)
        {
            services.AddTransient<IHRBirdService, HRBirdServices.Implementation.HRBirdService>();
            services.AddTransient<IBirdsSubmissionService, BirdsSubmissionService>();
            services.AddTransient<IHRBirdImageCDNService, HRBirdImageCDNService>();
            services.AddTransient<IHRPictureStorageService, HRPictureStorageService>();
            services.AddTransient<IPictureDataFormatter, PictureDataFormatter>();
            return services;
        }
    }
}
