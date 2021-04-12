using HRBirdsModelDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRBirdsSignalR.Interface
{
    public interface IHRPictureSignalRService
    {
        Task OnNewImageSubmittedAsync(HRBirdsSignalRNotificationDto data);
        Task OnThumbnailUpdatedAsync(HRBirdsSignalRNotificationDto data);
    }
}
