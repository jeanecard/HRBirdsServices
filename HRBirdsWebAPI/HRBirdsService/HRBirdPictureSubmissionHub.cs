﻿using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace HRBirdServices
{
    public class HRBirdPictureSubmissionHub : Hub
    {
        public Task BroadcastMessage(string name, string message) =>
           Clients.All.SendAsync("broadcastMessage", name, message);

        public Task Echo(string name, string message) =>
            Clients.Client(Context.ConnectionId)
                   .SendAsync("echo", name, $"{message} (echo from server)");
    }
}
