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
        public static readonly String THUMBNAIL_REST_END_POINT_ENV_KEY = "HRsignalRThumbnailImageRestEndPoint";
        public static readonly String NEW_IMAGE_REST_END_POINT_ENV_KEY = "HRsignalRNewImageRestEndPoint";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ressourceRest"></param>
        /// <returns></returns>
        public async Task NotifySignalRRestAsync(HRBirdsSignalRNotificationDto data, string ressourceRestEnvKey)
        {
            using (var client = new HttpClient())
            {
                var dataAsJson = JsonSerializer.Serialize(data);
                var requestContent = new StringContent(dataAsJson, Encoding.UTF8, "application/json");
                string endpoint = Environment.GetEnvironmentVariable(ressourceRestEnvKey);
                if (ressourceRestEnvKey == NEW_IMAGE_REST_END_POINT_ENV_KEY)
                {
                    endpoint = "https://hrbirdssignalrwebapi.azurewebsites.net/api/HRBirdsImage/onNewImageSubmitted";
                }
                else
                {
                    endpoint = "https://hrbirdssignalrwebapi.azurewebsites.net/api/HRBirdsImage/onThumbnailUpdated";

                }
                using var responseTask = client.PutAsync(
                    endpoint,
                    requestContent);
                await responseTask;
            }
        }
    }
}
