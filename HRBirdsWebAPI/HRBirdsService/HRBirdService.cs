
using AutoMapper;
using HRBirdServices.Interface;
using HRBirdsModel;
using HRBirdsRepository.Interface;
using HRCommonModel;
using HRCommonModels;
using System;
using System.Threading.Tasks;

namespace HRBirdServices.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    internal class HRBirdService : IHRBirdService
    {
        private readonly IHRBirdRepository _repo = null;
        private readonly IMapper _mapper;
        private HRBirdService()
        {
            //Dummy for DI.
        }

        public HRBirdService(IHRBirdRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// 1- Check for consistency and set default value if not supplied by caller.
        /// 2- Call repo.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageModel"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public async Task<PagingParameterOutModel<HRBirdMainOutput>> GetMainRecordsAsync(HRBirdMainInput query, PagingParameterInModel pageModel, HRSortingParamModel orderBy)
        {
            //1-
            if(_repo == null)
            {
                throw new Exception("repository not set.");
            }
            if(query == null)
            {
                query = GetDefaultMainInput();
            }
            if(pageModel == null)
            {
                pageModel = GetDefaultMainPagination();
            }
            //2-
            using (var taskRepo = _repo.GetMainRecordsAsync(query, pageModel, orderBy))
            {
                await taskRepo;
                if (taskRepo.IsCompletedSuccessfully)
                {
                    return taskRepo.Result;
                }
                else
                {
                    throw new Exception("Something goes wrong in _repo.GetMainRecordsAsync");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private PagingParameterInModel GetDefaultMainPagination()
        {
            PagingParameterInModel retour = new PagingParameterInModel() { PageNumber = 0, PageSize = 20 };
            return retour;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private HRBirdMainInput GetDefaultMainInput()
        {
            HRBirdMainInput retour = new HRBirdMainInput();
            retour.Lat = 0.0F;
            retour.Lon = 0.0F;
            retour.Range = 5000;
            retour.Season = HRSeason.all;
            retour.Lang_Iso_Code = "fr";
            return retour;
        }
    }
}
