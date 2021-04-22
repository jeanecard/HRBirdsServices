using HRBirdsModelDto;
using HRBirdsSignalR.Interface;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace HRBirdsSignalR.Service
{
    public class HRPictureSignalRService : IHRPictureSignalRService
    {
        private IHubContext<HRBirdPictureSubmissionHub> _informHub = null;


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
        public async Task OnNewImageSubmittedAsync(HRSubmitPictureListItemDto data)
        {
            if(data != null)
            {
                String jsonData = JsonSerializer.Serialize(data);
                using var notifyTask = _informHub.Clients.All.SendAsync(
                    HRBirdPictureSubmissionHub.CLIENT_NEW_IMAGE_NOTIFICATION_KEY,
                    jsonData);
                await notifyTask;
            }
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task OnThumbnailUpdatedAsync(HRSubmitPictureListItemDto data)
        {
            if (data != null)
            {
                String jsonData = JsonSerializer.Serialize(data);

                using var notifyTask = _informHub.Clients.All.SendAsync(
                    HRBirdPictureSubmissionHub.CLIENT_UPDATE_THUMBNAIL_NOTIFICATION_KEY,
                    jsonData);
                await notifyTask;
            }
        }

    }
}
