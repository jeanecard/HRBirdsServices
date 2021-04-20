using Azure.Storage.Queues;
using HRBirdsEntities;
using HRBirdService.Interface;
using HRBirdsModelDto;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HRBirdService
{
    public class SubmittedImageNotifier : ISubmittedImageNotifier
    {
        private readonly IOptions<HRAzureQueueConfig> _config = null;

        private SubmittedImageNotifier()
        {
            //Dummy for DI.
        }

        public SubmittedImageNotifier(IOptions<HRAzureQueueConfig> config)
        {
            _config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task OnNewMetadataImageAsync(HRSubmitPictureInputDto message)
        {
            if(message == null)
            {
                return;
            }
            // Get the connection string from app settings
            // Instantiate a QueueClient which will be used to create and manipulate the queue
            QueueClientOptions queueClientOptions = new QueueClientOptions()
            {
                MessageEncoding = QueueMessageEncoding.Base64
            };

            QueueClient queueClient = new QueueClient(
                _config.Value?.HRSubmittedPictureCx, 
                _config.Value?.HRNewImageMetadataQueueName,
                queueClientOptions);

            // Create the queue if it doesn't already exist
            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {

                String messageToString = JsonConvert.SerializeObject(message);

                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(messageToString);
                string message64 = System.Convert.ToBase64String(plainTextBytes);
                using var queueTask = queueClient.SendMessageAsync(message64);

                await queueTask;
                if (!queueTask.IsCompletedSuccessfully)
                {
                    throw new Exception("queueClient.SendMessageAsync failure.");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task OnThumbnailUpdatedAsync(HRSubmitPictureListItemDto message)
        {
            if (message == null)
            {
                return;
            }
            // Get the connection string from app settings
            // Instantiate a QueueClient which will be used to create and manipulate the queue
            QueueClientOptions queueClientOptions = new QueueClientOptions()
            {
                MessageEncoding = QueueMessageEncoding.Base64
            };

            QueueClient queueClient = new QueueClient(
                _config.Value?.HRSubmittedPictureCx,
                _config.Value?.HRUpdteThumbnailQueueName,
                queueClientOptions);

            // Create the queue if it doesn't already exist
            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {

                String messageToString = JsonConvert.SerializeObject(message);

                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(messageToString);
                string message64 = System.Convert.ToBase64String(plainTextBytes);
                using var queueTask = queueClient.SendMessageAsync(message64);

                await queueTask;
                if (!queueTask.IsCompletedSuccessfully)
                {
                    throw new Exception("queueClient.SendMessageAsync failure.");
                }
            }
        }
    }
}
