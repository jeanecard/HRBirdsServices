using HRBirdService.Interface;
using HRBirdsModelDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBirdsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HRPictureStorageController : ControllerBase
    {
        private IHRPictureStorageService _storageService = null;
        private readonly IBirdsSubmissionService _birdsSubmissionService = null; //TODO move in service

        private HRPictureStorageController()
        {
            // Dummy for DI.
        }

        public HRPictureStorageController(IHRPictureStorageService service, IBirdsSubmissionService subService)
        {
            _storageService = service;
            _birdsSubmissionService = subService;
        }
        // GET: api/<PictureStorageController>
       
        /// <summary>
        /// 1- Add a file in azure storage 
        /// 2- Update Picture Metadata with the new URL TODO move in service, no business in here 
        /// </summary>
        /// <param name="theFile"></param>
        /// <returns></returns>
        [HttpPost("add-picture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<HRSubmitPictureOutputDto>> Post([FromBody] FileToCreateDto theFile)
        {
            if(theFile == null 
                || theFile.SubmittedPicture == null
                || theFile.SubmittedPicture.Id == null
                || theFile.SubmittedPicture.Id == System.Guid.Empty)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            try
            {
                //1-
                using var uploadTask = _storageService.UploadAsync(theFile);
                await uploadTask;
                if(uploadTask.IsCompletedSuccessfully)
                {
                    //2-
                    HRSubmitPictureInputDto pictureToSubmit = new HRSubmitPictureInputDto();
                    pictureToSubmit.VernacularName = theFile.SubmittedPicture?.VernacularName;
                    pictureToSubmit.SourceType = theFile.SubmittedPicture?.SourceType;
                    pictureToSubmit.Id = theFile.SubmittedPicture.Id;
                    pictureToSubmit.GenderType = theFile.SubmittedPicture?.GenderType;
                    pictureToSubmit.Credit = theFile.SubmittedPicture?.Credit;
                    pictureToSubmit.Comment = theFile.SubmittedPicture?.Comment;
                    pictureToSubmit.AgeType = theFile.SubmittedPicture?.AgeType;
                    pictureToSubmit.ThumbnailUrl = theFile.SubmittedPicture?.ThumbnailUrl;
                    pictureToSubmit.FullImageUrl = uploadTask.Result;

                    using var subTask = _birdsSubmissionService.UpdatePictureDataAsync(pictureToSubmit);
                    await subTask;
                    if (subTask.IsCompletedSuccessfully)
                    {
                        return Ok(subTask.Result);
                    }
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
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
