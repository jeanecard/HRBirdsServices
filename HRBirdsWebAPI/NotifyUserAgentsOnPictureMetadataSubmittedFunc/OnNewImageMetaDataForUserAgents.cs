using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace NotifyUserAgentsOnPictureMetadataSubmittedFunc
{
    public static class OnNewImageMetaDataForUserAgents
    {
        [FunctionName("OnNewImageMetaDataForUserAgentsFunction")]
        public static void Run([QueueTrigger("hrnewimagemetadata", Connection = "HR_PICTURE_SUBMITTED_QUEUE")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
