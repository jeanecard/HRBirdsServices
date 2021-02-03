using HRBirdService.Interface;
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
    }
}
