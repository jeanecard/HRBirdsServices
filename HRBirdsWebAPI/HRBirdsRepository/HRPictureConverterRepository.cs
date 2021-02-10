using HRBirdEntity;
using HRBirdRepository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRBirdRepository
{
    internal class HRPictureConverterRepository : IHRPictureConverterRepository
    {
        public async Task<string> AddPictureAsync(HRSubmitPicture rawDataPicture)
        {
            await Task.Delay(1);
            return Guid.NewGuid().ToString() + ".jpg";
        }
    }
}
