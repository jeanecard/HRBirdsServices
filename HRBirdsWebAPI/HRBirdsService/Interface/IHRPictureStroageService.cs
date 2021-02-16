using HRBirdsModelDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRBirdService.Interface
{
    public interface IHRPictureStorageService
    {
        Task<String> UploadAsync(FileToCreateDto theFile);
    }
}
