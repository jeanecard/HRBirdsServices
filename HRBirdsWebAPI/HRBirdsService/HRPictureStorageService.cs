using AutoMapper;
using Azure.Storage;
using Azure.Storage.Blobs;
using HRBirdRepository.Interface;
using HRBirdsEntities;
using HRBirdService.Interface;
using HRBirdsModelDto;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HRBirdService
{
    internal class HRPictureStorageService : IHRPictureStorageService
    {
        private readonly IPictureDataFormatter _picFormatter = null;
        private readonly IOptions<HRAzureBlobConfig> _config = null;
        private readonly IMapper _mapper = null;
        private readonly IHRBirdSubmissionRepository _repo = null;
        private readonly IHRImageNotifySignalR _notifier = null;

        private HRPictureStorageService()
        {
            //Dummy for DI.
        }

        public HRPictureStorageService(
            IPictureDataFormatter formatter,
            IHRBirdSubmissionRepository repo,
            IOptions<HRAzureBlobConfig> config,
            IHRImageNotifySignalR notifier,
            IMapper mapper)
        {
            _picFormatter = formatter;
            _config = config;
            _mapper = mapper;
            _repo = repo;
            _notifier = notifier;
        }
        /// <summary>
        /// Very first version, no segmentation management.
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
            try
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
                    String blobPath = rootUrl + directoryTask.Result  +
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
                        //2-
                        using var notifyResult = _notifier.NotifySignalRRestAsync(
                            new HRBirdsSignalRNotificationDto()
                            {
                                Id = theFile.SubmittedPicture.Id.Value,
                                Url = blobPath,
                                VernacularName = theFile.SubmittedPicture.VernacularName
                            },
                            HRImageNotifySignalR.NEW_IMAGE_REST_END_POINT_ENV_KEY
                            );
                        await notifyResult;
                        if (notifyResult.IsCompletedSuccessfully)
                        {
                            return blobPath;
                        }
                        else
                        {
                            throw new Exception("_notifier.NotifySignalRRestAsync fail");
                        }
                    }
                    else
                    {
                        throw new Exception("blobClient.UploadAsync fail");
                    }
                }
                else
                {
                    throw new Exception("_picFormatter.GetPathAsync fail");
                }
            }
            catch (Exception)
            {
            
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullImageURL"></param>
        /// <param name="thumbnail"></param>
        /// <returns></returns>
        public async Task UpdateThumbnailAsync(string id, string thumbnail)
        {
            //1- Updatethumbnails in repo
            using var updateTask = _repo.UpdateThumbnailAsync(id, thumbnail);
            await updateTask;
            if(updateTask.IsCompletedSuccessfully)
            {
                //2- GetAll updated images
                using var getTask = _repo.GetSubmittedPicturesByID(id);
                await getTask;
                if(getTask.IsCompletedSuccessfully)
                {
                    //TODO use automappper
                        var message = new HRBirdsSignalRNotificationDto()
                        {
                            Id = new Guid(id),
                            Url = thumbnail,
                            VernacularName = getTask.Result.VernacularName
                        };
                        //For each updated items, send notification
                        using var notifyTask = _notifier.NotifySignalRRestAsync(
                            message, 
                            HRImageNotifySignalR.THUMBNAIL_REST_END_POINT_ENV_KEY);
                        await notifyTask;
                }
                else
                {
                    throw new Exception("GetSubmittedPicturesByFullImageUrlAsync failed.");
                }
            }
            else
            {
                throw new Exception("UpdateThumbnailAsync failed.");
            }
        }
    }
}
