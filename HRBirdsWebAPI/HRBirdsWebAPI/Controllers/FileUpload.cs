using Azure.Storage;
using Azure.Storage.Blobs;
using HRBirdsModelDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBirdsWebAPI.Controllers
{
    [Route("api/FileUpload")]
    [ApiController]
    public class FileUpload : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FileToUpload theFile)
        {

            if (theFile.FileAsBase64.Contains(","))
            {
                theFile.FileAsBase64 = theFile.FileAsBase64.Substring(theFile.FileAsBase64.IndexOf(",") + 1);
            }
            theFile.FileAsByteArray = Convert.FromBase64String(theFile.FileAsBase64);
            using (var fs = new MemoryStream(theFile.FileAsByteArray))
            {
                Uri blobUri = new Uri("https://" +
                                 "hrbirdsblobstorage" +
                                 ".blob.core.windows.net/" +
                                 "fullimage" +
                                 "/" + "rastos/saucisse2.jpeg");

                // Create StorageSharedKeyCredentials object by reading
                // the values from the configuration (appsettings.json)
                StorageSharedKeyCredential storageCredentials =
                    new StorageSharedKeyCredential("hrbirdsblobstorage", "tXCZ1DtJHdWDy+7GiVNFM0lkXb5/RRFvq91/h4MR688uisqXqPD41YD1C2WXhFRZ8yEDNfCeqbWG8iZutsVhTA==");

                // Create the blob client.
                BlobClient blobClient = new BlobClient(blobUri, storageCredentials);

                // Upload the file
                var suacisseTask = blobClient.UploadAsync(fs);
                await suacisseTask;

            }
            return Ok();
        }
    }
}
