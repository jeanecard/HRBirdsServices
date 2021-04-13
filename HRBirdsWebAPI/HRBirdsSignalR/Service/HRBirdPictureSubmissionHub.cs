using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace HRBirdsSignalR.Service
{
    public class HRBirdPictureSubmissionHub : Hub
    {
        public static readonly string CLIENT_UPDATE_THUMBNAIL_NOTIFICATION_KEY = "ThumbnailUpdated";
        public static readonly string CLIENT_NEW_IMAGE_NOTIFICATION_KEY = "ImageCreated";
        public static readonly string CLIENT_NOTIFICATION_KEY = "ConnectionDone";


        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync(CLIENT_NOTIFICATION_KEY, "Connected successfully!");
        }
    }
}
