using HRBirdsModelDto;
using HRBirdsSignalR.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBirdsSignalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HRBirdsImageController : ControllerBase
    {
        private readonly IHRPictureSignalRService _signalRService = null;

        private HRBirdsImageController()
        {
            //Dummy for DI.
        }

        public HRBirdsImageController(IHRPictureSignalRService service)
        {
            _signalRService = service;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("onThumbnailUpdated")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> OnThumbnailUpdatedAsync([FromBody] HRSubmitPictureInputDto data)
        {
            if (data == null
                || String.IsNullOrEmpty(data.VernacularName)
                || data.Id == null
                || data.Id == Guid.Empty)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            try
            {
                using var updateTask = _signalRService.OnThumbnailUpdatedAsync(data);
                await updateTask;
                if(updateTask.IsCompletedSuccessfully)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("onNewImageSubmitted")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> onNewImageSubmittedAsync([FromBody] HRSubmitPictureInputDto data)
        {
            if (data == null
                || String.IsNullOrEmpty(data.VernacularName)
                || data.Id == null
                || data.Id == Guid.Empty)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            try
            {
                using var newTask = _signalRService.OnNewImageSubmittedAsync(data);
                await newTask;
                if (newTask.IsCompletedSuccessfully)
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
