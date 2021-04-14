using HRBirdEntity;
using HRBirdService.Interface;
using HRBirdsModelDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBordersAndCountriesWebAPI2.Controllers
{
    /// <summary>
    /// Controller to submit pictures and sounds
    /// </summary>
    [Route("api/v1.0/HRBirdSubmission")]
    [ApiController]
    public class HRBirdSubmissionController : ControllerBase
    {
        private readonly IBirdsSubmissionService _birdsSubmissionService = null;

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
        /// Get matching vernacular name's beyond already submitted data and official existing ones.
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
        /// Get all images data (thumbnailed) for a given vernacular name.
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

        [HttpGet("get-image/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<HRSubmitPictureListItemDto>> GetImageAsync([FromRoute] string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            try
            {
                using var taskResult = _birdsSubmissionService.GetSubmittedPictureAsync(id);
                await taskResult;
                if (taskResult.IsCompletedSuccessfully)
                {
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
        /// Add image metat-data (not image itself uploaded via HRPictureStroageController)
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        [HttpPost("add-image-metadata")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<HRSubmitPictureOutputDto>> Post([FromBody] HRSubmitPictureInputDto picture)
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
        /// Update image metat-data (not image itself uploaded via HRPictureStroageController)
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        [HttpPut("update-image-metadata")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<HRSubmitPictureOutputDto>> Put([FromBody] HRSubmitPictureInputDto picture)
        {
            if (picture == null)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            try
            {
                using var taskResult = _birdsSubmissionService.UpdatePictureDataAsync(picture);
                await taskResult;
                if (taskResult.IsCompletedSuccessfully)
                {
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
        /// Delete image meta data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete-image-metadata/{id}")]
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
