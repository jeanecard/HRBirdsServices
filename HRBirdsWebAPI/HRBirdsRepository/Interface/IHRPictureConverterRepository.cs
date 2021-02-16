using HRBirdEntity;
using System;
using System.Threading.Tasks;

namespace HRBirdRepository.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHRPictureConverterRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawDataPicture"></param>
        /// <returns></returns>
        Task<String> AddPictureAsync(HRSubmitPictureInput rawDataPicture);
    }
}
