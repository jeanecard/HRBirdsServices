using HRBirdService.Interface;
using HRBirdsModelDto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HRBirdService
{
    public class HRImageNotifySignalR : IHRImageNotifySignalR
    {
        public static readonly String THUMBNAIL_REST_END_POINT = "HRsignalRThumbnailImageRestEndPoint";
        public static readonly String NEW_IMAGE_REST_END_POINT = "HRsignalRNewImageRestEndPoint";
        public static readonly String SIGNALR_ENDPOINT_KEY = "HRSignalR_Endpoint";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ressourceRest"></param>
        /// <returns></returns>
        public async Task NotifySignalRRestAsync(HRBirdsSignalRNotificationDto data, string ressourceRest)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var dataAsJson = JsonSerializer.Serialize(data);
                    var requestContent = new StringContent(dataAsJson, Encoding.UTF8, "application/json");
                    using var responseTask = client.PutAsync(Environment.GetEnvironmentVariable(SIGNALR_ENDPOINT_KEY) + ressourceRest, requestContent);
                    //using var responseTask = client.PutAsync("https://hrbirdssignalrwebapi.azurewebsites.net/api/HRBirdsImage/" + ressourceRest, requestContent);

                
                    await responseTask;
                }
                catch(Exception ex)
                {
                    int i = 41;
                    i++;
                }
            }
        }
    }
}
