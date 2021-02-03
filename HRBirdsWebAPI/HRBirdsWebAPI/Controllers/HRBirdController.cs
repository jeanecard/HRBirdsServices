
using HRBirdServices.Interface;
using HRBirdsModel;
using HRCommonModel;
using HRCommonModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<HRBirdController> _logger = null;
        private readonly IHRBirdService _birdService = null;
        private readonly IConfiguration _config = null;

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
        public HRBirdController(IConfiguration config,
            IHRBirdService birdService,
            ILogger<HRBirdController> logger)
        {
            _config = config;
            _birdService = birdService;
            _logger = logger;
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
        public async Task<ActionResult<PagingParameterOutModel<HRBirdMainOutput>>>  Get(
            [FromQuery] HRBirdMainInput query, 
            [FromQuery] PagingParameterInModel pageModel,
            [FromQuery]  HRSortingParamModel orderBy)
        {
            using (var resultTask = _birdService.GetMainRecordsAsync(query, pageModel, orderBy))
            {
                await resultTask;
                return resultTask.Result;
            }
        }
    }
}
