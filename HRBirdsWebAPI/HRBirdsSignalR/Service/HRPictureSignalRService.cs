using HRBirdsModelDto;
using HRBirdsSignalR.Interface;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace HRBirdsSignalR.Service
{
    public class HRPictureSignalRService : IHRPictureSignalRService
    {
        private IHubContext<HRBirdPictureSubmissionHub> _informHub;

        private HRPictureSignalRService()
        {
            //Dummy for DI.
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="hubContext"></param>
        public HRPictureSignalRService(
           IHubContext<HRBirdPictureSubmissionHub> hubContext)
        {
            _informHub = hubContext;
        }
        public async Task OnNewImageSubmittedAsync(HRBirdsSignalRNotificationDto data)
        {
            if(data != null)
            {
                using var notifyTask = _informHub.Clients.All.SendAsync(HRBirdPictureSubmissionHub.CLIENT_NOTIFICATION_KEY, "le GUID qui va bien");
                await notifyTask;
            }
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task OnThumbnailUpdatedAsync(HRBirdsSignalRNotificationDto data)
        {
            if (data != null)
            {
                using var notifyTask = _informHub.Clients.All.SendAsync(HRBirdPictureSubmissionHub.CLIENT_NOTIFICATION_KEY, "le GUID qui va bien");
                await notifyTask;
            }
        }
    }
}
