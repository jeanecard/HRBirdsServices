using Azure.Storage;
using Azure.Storage.Blobs;
using HRBirdEntity;
using HRBirdService.Interface;
using HRBirdsModelDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBordersAndCountriesWebAPI2.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class HRBirdSubmissionController : ControllerBase
    {
        private IBirdsSubmissionService _birdsSubmissionService = null;

        private HRBirdSubmissionController()
        {
            // Dummy for DI.
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="service"></param>
        public HRBirdSubmissionController(IBirdsSubmissionService service)
        {
            _birdsSubmissionService = service;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        [HttpGet("matching-names/{pattern}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<String>>> GetMatchingVernacularNamesAsync([FromRoute] string pattern)
        {
            if(String.IsNullOrEmpty(pattern))
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            try
            {
                using var taskResult = _birdsSubmissionService.GetMatchingVernacularNamesAsync(pattern);
                await taskResult;
                if(taskResult.IsCompletedSuccessfully)
                {
                    if(taskResult.Result == null || taskResult.Result.Count() == 0)
                    {
                        return new StatusCodeResult(StatusCodes.Status204NoContent);

                    }
                    return Ok(taskResult.Result);
                } 
                else
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        [HttpGet("get-images/{vernacularName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<HRSubmitPictureListItemDto>>> GetImagesAsync([FromRoute] string vernacularName)
        {
            if (String.IsNullOrEmpty(vernacularName))
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            try
            {
                using var taskResult = _birdsSubmissionService.GetSubmittedPicturesAsync(vernacularName);
                await taskResult;
                if (taskResult.IsCompletedSuccessfully)
                {
                    if (taskResult.Result == null || taskResult.Result.Count() == 0)
                    {
                        return new StatusCodeResult(StatusCodes.Status204NoContent);

                    }
                    return Ok(taskResult.Result);
                }
                else
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("add-image-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> Post([FromBody] HRSubmitPictureInput picture)
        {
            if (picture == null)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            try
            {
                using var taskResult = _birdsSubmissionService.AddPictureDataAsync(picture);
                await taskResult;
                if (taskResult.IsCompletedSuccessfully)
                {
                    return Ok();
                }
                else
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("add-picture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                                 "/" + "rastos/" + theFile.FileName);

                // Create StorageSharedKeyCredentials object by reading
                // the values from the configuration (appsettings.json)
                StorageSharedKeyCredential storageCredentials =
                    new StorageSharedKeyCredential("nomarecuperer", "motdepassearecuperer");

                // Create the blob client.
                BlobClient blobClient = new BlobClient(blobUri, storageCredentials);

                // Upload the file
                var suacisseTask = blobClient.UploadAsync(fs);
                await suacisseTask;

            }
            return Ok();
        }

        [HttpDelete("delete-image-data/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteImageDataAsync([FromRoute] Guid id)
        {
            try
            {
                using var taskResult = _birdsSubmissionService.DeletePictureDataAsync(id);
                await taskResult;
                if (taskResult.IsCompletedSuccessfully)
                {
                      return Ok();
                }
                else
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
