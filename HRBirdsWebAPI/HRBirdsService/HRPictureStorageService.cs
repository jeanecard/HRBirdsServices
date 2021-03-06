﻿using AutoMapper;
using Azure.Storage;
using Azure.Storage.Blobs;
using HRBirdRepository.Interface;
using HRBirdsEntities;
using HRBirdService.Interface;
using HRBirdsModelDto;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRBirdService
{
    internal class HRPictureStorageService : IHRPictureStorageService
    {
        private readonly IPictureDataFormatter _picFormatter = null;
        private readonly IOptions<HRAzureBlobConfig> _config = null;
        private readonly IMapper _mapper = null;
        private readonly IHRBirdSubmissionRepository _repo = null;
        private ISubmittedImageNotifier _queueService = null;

        private HRPictureStorageService()
        {
            //Dummy for DI.
        }

        public HRPictureStorageService(
            IPictureDataFormatter formatter,
            IHRBirdSubmissionRepository repo,
            IOptions<HRAzureBlobConfig> config,
            IHRImageNotifySignalR notifier,
            ISubmittedImageNotifier queueservice,
            IMapper mapper)
        {
            _picFormatter = formatter;
            _config = config;
            _mapper = mapper;
            _repo = repo;
            _queueService = queueservice;
        }
        /// <summary>
        /// 1- upload in Blob storage
        /// 2- queue new message 
        /// 3- Temporary cheat, wake up listening Azure functions (fire and forget)
        /// </summary>
        /// <param name="theFile"></param>
        /// <returns></returns>
        public async Task<string> UploadAsync(FileToCreateDto theFile)
        {
            if (theFile == null
                || String.IsNullOrEmpty(theFile.FileAsBase64)
                || theFile.SubmittedPicture == null
                || theFile.SubmittedPicture.Id == Guid.Empty)
            {
                throw new ArgumentNullException();
            }
            //1-
            using var blobTask = UploadInBlobAsync(theFile);
            await blobTask;
            if (blobTask.IsCompletedSuccessfully)
            {
                theFile.SubmittedPicture.FullImageUrl = blobTask.Result;
                //2-
                using var getTask = _repo.GetSubmittedPicturesByIDAsync(theFile.SubmittedPicture.Id.ToString());
                await getTask;
                if (getTask.IsCompletedSuccessfully)
                {
                    var itemToQueue = _mapper.Map<HRSubmitPictureListItemDto>(getTask.Result);
                    itemToQueue.FullImageUrl = blobTask.Result;
                    using var queueTask = _queueService.OnNewImageAsync(itemToQueue);
                    await queueTask;
                    if (queueTask.IsCompletedSuccessfully)
                    {
                        //3-
                        WakeUpAzureFunction();
                        return blobTask.Result;
                    }
                    else
                    {
                        throw new Exception("UploadInBlobAsync fail");
                    }
                }
                else
                {
                    throw new Exception("_repo.GetSubmittedPicturesByIDAsync fail");
                }
            }
            else
            {
                throw new Exception("UploadInBlobAsync fail");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void WakeUpAzureFunction()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    String endpoint = _config?.Value?.WakeUpAzureFunctionEndPoint; 
                    var response = client.GetAsync(endpoint);
                }
                catch
                {
                    //Dummy
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theFile"></param>
        /// <returns></returns>
        private async Task<String> UploadInBlobAsync(FileToCreateDto theFile)
        {
            FileToCreate fToCreate = _mapper.Map<FileToCreate>(theFile);
            fToCreate.FileAsBase64 = _picFormatter.CleanData(theFile.FileAsBase64);
            fToCreate.FileAsByteArray = Convert.FromBase64String(fToCreate.FileAsBase64);

            var directoryTask = _picFormatter.GetPathAsync(fToCreate);
            await directoryTask;
            if (directoryTask.IsCompletedSuccessfully)
            {

                using var fs = new MemoryStream(fToCreate.FileAsByteArray);
                String rootUrl = _config.Value?.BlobRootURI;
                String blobPath = rootUrl + directoryTask.Result +
                        theFile.SubmittedPicture.Id.ToString() +
                        "." +
                        "jpeg";
                Uri blobUri = new Uri(blobPath);

                // Create StorageSharedKeyCredentials object by reading
                // the values from the configuration (appsettings.json)
                String credNameValue = _config?.Value?.StorageSharedKeyCredentialName;
                String pwd = _config?.Value?.Password;
                StorageSharedKeyCredential storageCredentials =
                    new StorageSharedKeyCredential(credNameValue, pwd);

                // Create the blob client.
                BlobClient blobClient = new BlobClient(blobUri, storageCredentials);
                // Upload the file
                var uploadTask = blobClient.UploadAsync(fs);
                await uploadTask;
                if (uploadTask.IsCompletedSuccessfully)
                {
                    return blobPath;
                }
                else
                {
                    throw new Exception("blobClient.UploadAsync failed.");
                }
            }
            else
            {
                throw new Exception("_picFormatter.GetPathAsync failed.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullImageURL"></param>
        /// <param name="thumbnail"></param>
        /// <returns></returns>
        public async Task UpdateThumbnailAsync(Guid id, string thumbnail)
        {
            //1- Updatethumbnails in repo
            using var updateTask = _repo.UpdateThumbnailAsync(id, thumbnail);
            await updateTask;
            if (!updateTask.IsCompletedSuccessfully)
            {
                throw new Exception("UpdateThumbnailAsync failed.");
            }
        }
    }
}
