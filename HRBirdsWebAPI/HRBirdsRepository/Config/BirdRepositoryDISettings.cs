using HRBirdRepository.Interface;
using HRBirdsRepository.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRBirdRepository
{
    public static class BirdRepositoryDISettings
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDI(IServiceCollection services)
        {
            services.AddTransient<IHRBirdRepository, HRBirdsRepository.Implementation.HRBirdRepository>();
            services.AddTransient<IHRBirdSubmissionRepository, HRBirdSubmissionRepository>();
            services.AddTransient<IHRPictureConverterRepository, HRPictureConverterRepository>();
            return services;
        }
    }
}
