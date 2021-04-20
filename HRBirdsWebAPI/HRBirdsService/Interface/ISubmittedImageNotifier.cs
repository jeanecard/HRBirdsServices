using HRBirdsModelDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRBirdService.Interface
{
    public interface ISubmittedImageNotifier
    {
        Task OnNewMetadataImageAsync(HRSubmitPictureInputDto message);
        Task OnThumbnailUpdatedAsync(HRSubmitPictureListItemDto message);

    }
}
