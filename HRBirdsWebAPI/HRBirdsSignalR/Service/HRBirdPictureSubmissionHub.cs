using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace HRBirdsSignalR.Service
{
    public class HRBirdPictureSubmissionHub : Hub
    {
        public static readonly string CLIENT_NOTIFICATION_KEY = "Message";
        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync(CLIENT_NOTIFICATION_KEY, "Connected successfully!");
        }
    }
}
