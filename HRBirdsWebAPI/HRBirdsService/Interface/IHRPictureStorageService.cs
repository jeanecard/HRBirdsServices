using HRBirdsModelDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRBirdService.Interface
{
    public interface IHRPictureStorageService
    {
        Task<String> UploadAsync(FileToCreateDto theFile);
        Task UpdateThumbnailAsync(String fullImageURL, String thumbnail);
    }
}
