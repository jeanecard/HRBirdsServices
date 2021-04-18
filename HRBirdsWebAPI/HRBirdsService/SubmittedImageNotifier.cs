using Azure.Storage.Queues;
using HRBirdsEntities;
using HRBirdService.Interface;
using HRBirdsModelDto;
using Microsoft.Extensions.Options;
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
            QueueClient queueClient = new QueueClient(
                _config.Value?.HRSubmittedPictureCx, 
                _config.Value?.HRNewImageMetadataQueueName);

            // Create the queue if it doesn't already exist
            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer
                    (message.GetType());

                using (StringWriter textWriter = new StringWriter())
                {
                    x.Serialize(textWriter, message);
                    using var queueTask = queueClient.SendMessageAsync(textWriter.ToString());
                    await queueTask;
                    if(!queueTask.IsCompletedSuccessfully)
                    {
                        throw new Exception("queueClient.SendMessageAsync failure.");
                    }
                }
            }
        }
    }
}
