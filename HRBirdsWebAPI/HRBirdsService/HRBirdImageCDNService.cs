using HRBirdService.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRBirdService
{
    internal class HRBirdImageCDNService : IHRBirdImageCDNService
    {
        /// <summary>
        /// Very first test version
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetImageDataProcessingUrlAsync()
        {
            await Task.Delay(1);
            return @"https://jeanecard.github.io/HRBirdsPicturesDB/HRAPPS-CDN/image-processing.svg";
        }
    }
}
