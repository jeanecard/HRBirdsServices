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

        private HRPictureStorageService()
        {
            //Dummy for DI.
        }

        public HRPictureStorageService(
            IPictureDataFormatter formatter,
            IHRBirdSubmissionRepository repo,
            IOptions<HRAzureBlobConfig> config,
            IMapper mapper)
        {
            _picFormatter = formatter;
            _config = config;
            _mapper = mapper;
            _repo = repo;
        }
        /// <summary>
        /// Very first version, no segmentation management.
        /// </summary>
        /// <param name="theFile"></param>
        /// <returns></returns>
        public async Task<string> UploadAsync(FileToCreateDto theFile)
        {
            if (theFile == null || String.IsNullOrEmpty(theFile.FileAsBase64))
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

                    String blobPath =  _config.Value?.BlobRootURI + directoryTask.Result  +
                            Guid.NewGuid().ToString() +
                            "." +
                            "jpeg";
                    Uri blobUri = new Uri(blobPath);

                    // Create StorageSharedKeyCredentials object by reading
                    // the values from the configuration (appsettings.json)
                    StorageSharedKeyCredential storageCredentials =
                        new StorageSharedKeyCredential(_config?.Value?.StorageSharedKeyCredentialName, _config?.Value?.Password);

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
        public async Task UpdateThumbnailAsync(string fullImageURL, string thumbnail)
        {
            //1- Updatethumbnails in repo
            using var updateTask = _repo.UpdateThumbnailAsync(fullImageURL, thumbnail);
            await updateTask;
            if(updateTask.IsCompletedSuccessfully)
            {
                //2- GetAll updated image's Id
                //3- Foreach images updated, trigger signalR imageupdate
            }
        }
    }
}
