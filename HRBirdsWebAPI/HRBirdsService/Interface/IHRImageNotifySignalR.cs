using HRBirdsModelDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRBirdService.Interface
{
    public interface IHRImageNotifySignalR
    {

        Task NotifySignalRRestAsync(HRBirdsSignalRNotificationDto data, string ressourceRest);
    }
}
