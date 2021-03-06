﻿using HRBirdsModelDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRBirdsSignalR.Interface
{
    public interface IHRPictureSignalRService
    {
        Task OnNewImageSubmittedAsync(HRSubmitPictureListItemDto data);
        Task OnThumbnailUpdatedAsync(HRSubmitPictureListItemDto data);
    }
}
