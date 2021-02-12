using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRBirdService.Interface
{
    public interface IHRBirdImageCDNService
    {
        public Task<String> GetImageDataProcessingUrlAsync();
    }
}
