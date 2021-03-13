
using HRBirdServices.Interface;
using HRBirdsModel;
using HRCommonModel;
using HRCommonModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRBordersAndCountriesWebAPI2.Controllers
{

    /// <summary>
    ///Controller for main information on Birds
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class HRBirdController : ControllerBase
    {
        private readonly IHRBirdService _birdService = null;

        private HRBirdController()
        {
            //Dummy for DI.
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="birdService"></param>
        /// <param name="util"></param>
        /// <param name="logger"></param>
        public HRBirdController(IHRBirdService birdService)
        {
            _birdService = birdService;
        }


        /// <summary>
        /// Get all main records of Birds matching the input query
        /// </summary>
        /// <param name="query">Input query. Can be null.</param>
        /// <param name="pageModel">page model. Can be null.</param>
        /// <param name="orderBy">order by query paging. Can be null.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status416RequestedRangeNotSatisfiable)]
        [ProducesResponseType(StatusCodes.Status413PayloadTooLarge)]
        public async Task<ActionResult<PagingParameterOutModel<HRBirdMainOutput>>> Get(
            [FromQuery] HRBirdMainInput query,
            [FromQuery] PagingParameterInModel pageModel,
            [FromQuery] HRSortingParamModel orderBy)
        {
            using var resultTask = _birdService.GetMainRecordsAsync(query, pageModel, orderBy);
            await resultTask;
            return resultTask.Result;
        }
    }
}
