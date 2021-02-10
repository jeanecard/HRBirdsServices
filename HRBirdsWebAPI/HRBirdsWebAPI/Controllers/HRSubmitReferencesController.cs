using HRBirdService.Interface;
using HRBirdsModelDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBirdsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HRSubmitReferencesController : ControllerBase
    {
        private IBirdsSubmissionService _birdsSubmissionService = null;


        private HRSubmitReferencesController()
        {
            //Dummy for DI.
        }



        public HRSubmitReferencesController(IBirdsSubmissionService service)
        {
            _birdsSubmissionService = service;
        }
            
        [HttpGet("gender-types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<HRSubmitGenderDto>>> GetGenderTypes()
        {
            try
            {
                using Task<IEnumerable<HRSubmitGenderDto>> taskResult = _birdsSubmissionService.GetGenderTypesAsync();
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("age-types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<HRSubmitAgeDto>>> GetAgeTypes()
        {
            try
            {
                using Task<IEnumerable<HRSubmitAgeDto>> taskResult = _birdsSubmissionService.GetAgeTypesAsync();
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("sources")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<HRSubmitSourceDto>>> GetSourcesAsync()
        {
            try
            {
                using Task<IEnumerable<HRSubmitSourceDto>> taskResult = _birdsSubmissionService.GetSourcesAsync();
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

        ///
    }
}
