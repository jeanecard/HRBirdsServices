using AutoMapper;
using HRBirdEntity;
using HRBirdRepository.Interface;
using HRBirdsEntities;
using HRBirdService.Interface;
using HRBirdsModelDto;
using HRBirdsRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRBirdService
{
    public class BirdsSubmissionService : IBirdsSubmissionService
    {
        private readonly IHRBirdRepository _birdsRepo = null;
        private readonly IHRBirdSubmissionRepository _birdsSubmissionrepo = null;
        private readonly IHRPictureConverterRepository _birdsPictureConverter = null;
        private readonly IMapper _mapper = null;
        private readonly IHRBirdImageCDNService _imgCDNService = null;
        private ISubmittedImageNotifier _queueService = null;


        private BirdsSubmissionService()
        {
            // Dummy for DI.
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="bRepo"></param>
        /// <param name="bSubRepo"></param>
        public BirdsSubmissionService(
            IHRBirdRepository bRepo,
            IHRBirdSubmissionRepository bSubRepo,
            IHRPictureConverterRepository picConverter,
            IHRBirdImageCDNService cdn,
        ISubmittedImageNotifier queueService,
        IMapper mapper)
        {
            _birdsRepo = bRepo;
            _birdsSubmissionrepo = bSubRepo;
            _birdsPictureConverter = picConverter;
            _mapper = mapper;
            _imgCDNService = cdn;
            _queueService = queueService;

        }

        /// <summary>
        /// Merge of birds Repo and Birds Submission repo
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetMatchingVernacularNamesAsync(string pattern)
        {
            HashSet<string> vernacularNames = new HashSet<string>();
            //1- Get from existing birds
            using var birdsTask = _birdsRepo.GetMatchingVernacularNamesAsync(pattern);
            await birdsTask;
            if (birdsTask.IsCompletedSuccessfully)
            {
                foreach (String iter in birdsTask.Result)
                {
                    if (!vernacularNames.Contains(iter))
                    {
                        vernacularNames.Add(iter);
                    }
                }
            }
            else
            {
                throw new Exception("_birdsRepo.GetMatchingVernacularNamesAsync error");
            }
            //2- Get from birsSubmissionRepo
            using var birdsSubmissionTask = _birdsSubmissionrepo.GetMatchingVernacularNamesAsync(pattern);
            await birdsSubmissionTask;
            if (birdsSubmissionTask.IsCompletedSuccessfully)
            {
                foreach (String iter in birdsSubmissionTask.Result)
                {
                    if (!vernacularNames.Contains(iter))
                    {
                        vernacularNames.Add(iter);
                    }
                }
            }
            else
            {
                throw new Exception("_birdsSubmissionrepo.GetMatchingVernacularNamesAsync error");
            }
            //3- sort and return 
            return vernacularNames.OrderBy(name => name);

        }
        /// <summary>
        /// 1- Submit picture to image Repository
        /// 2- Push new imqge metadata to Queue
        /// 3- Return image
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public async Task<HRSubmitPictureOutputDto> AddPictureDataAsync(HRSubmitPictureInputDto picture)
        {
            if (picture != null)
            {
                //1- 
                using var thumbnailTask = _imgCDNService.GetImageDataProcessingUrlAsync();
                await thumbnailTask;
                if (thumbnailTask.IsCompletedSuccessfully)
                {
                    var transcoPic = _mapper.Map<HRSubmitPictureInput>(picture);
                    transcoPic.ThumbnailUrl = thumbnailTask.Result;
                    using var taskPicture = _birdsSubmissionrepo.AddPictureAsync(transcoPic);
                    await taskPicture;
                    if (taskPicture.IsCompletedSuccessfully)
                    {
                        //2-
                        using var queuetask = _queueService.OnNewMetadataImageAsync(picture);
                        await queuetask;
                        //3-
                        return _mapper.Map<HRSubmitPictureOutputDto>(taskPicture.Result);
                    }
                    else
                    {
                        throw new Exception("_birdsPictureConverter.AddPictureAsync fail.");
                    }
                }
                else
                {
                    throw new Exception("_imgCDNService.GetImageDataProcessingUrlAsync fail.");
                }
            }
            return null;
        }
        /// <summary>
        /// TODO DAPPER
        /// </summary>
        /// <param name="vernacularName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HRSubmitPictureListItemDto>> GetSubmittedPicturesAsync(string vernacularName)
        {
            using var birdsSubmissionTask = _birdsSubmissionrepo.GetSubmittedPicturesAsync(vernacularName);
            await birdsSubmissionTask;
            if (birdsSubmissionTask.IsCompletedSuccessfully)
            {
                return _mapper.Map<IEnumerable<HRSubmitPictureListItemDto>>(birdsSubmissionTask.Result);
            }
            else
            {
                throw new Exception("_birdsSubmissionrepo.GetSubmittedPicturesAsync error");
            }
        }

        public async Task<HRSubmitPictureListItemDto> GetSubmittedPictureAsync(String id)
        {
            using var birdSubmissionTask = _birdsSubmissionrepo.GetSubmittedPicturesByID(id);
            await birdSubmissionTask;
            if (birdSubmissionTask.IsCompletedSuccessfully)
            {
                return _mapper.Map<HRSubmitPictureListItemDto>(birdSubmissionTask.Result);
            }
            else
            {
                throw new Exception("_birdsSubmissionrepo.GetSubmittedPictureAsync error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<HRSubmitGenderDto>> GetGenderTypesAsync()
        {
            using Task<IEnumerable<HRSubmitGender>> getTask = _birdsSubmissionrepo.GetGenderTypesAsync();
            await getTask;
            if (getTask.IsCompletedSuccessfully)
            {
                return _mapper.Map<IEnumerable<HRSubmitGenderDto>>(getTask.Result);
            }
            throw new Exception("Error onasync call : GetGenderTypesAsync");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<HRSubmitAgeDto>> GetAgeTypesAsync()
        {
            using Task<IEnumerable<HRSubmitAge>> getTask = _birdsSubmissionrepo.GetAgeTypesAsync();
            await getTask;
            if (getTask.IsCompletedSuccessfully)
            {
                return _mapper.Map<IEnumerable<HRSubmitAgeDto>>(getTask.Result);
            }
            throw new Exception("Error onasync call : GetAgeTypesAsync");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<HRSubmitSourceDto>> GetSourcesAsync()
        {
            using Task<IEnumerable<HRSubmitSource>> getTask = _birdsSubmissionrepo.GetSourcesAsync();
            await getTask;
            if (getTask.IsCompletedSuccessfully)
            {
                return _mapper.Map<IEnumerable<HRSubmitSourceDto>>(getTask.Result);
            }
            throw new Exception("Error onasync call : GetAgeTypesAsync");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePictureDataAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException();
            }
            using var deleteTask = _birdsSubmissionrepo.DeletePictureAsync(id);
            await deleteTask;
            if (!deleteTask.IsCompletedSuccessfully)
            {
                throw new Exception("_birdsSubmissionrepo.DeletePictureAsync fail.");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public async Task<HRSubmitPictureOutputDto> UpdatePictureDataAsync(HRSubmitPictureInputDto pictureInput)
        {
            if (pictureInput != null)
            {
                var transcoPic = _mapper.Map<HRSubmitPictureInput>(pictureInput);
                using var taskPicture = _birdsSubmissionrepo.UpdatePictureAsync(transcoPic);
                await taskPicture;
                if (taskPicture.IsCompletedSuccessfully)
                {
                    return _mapper.Map<HRSubmitPictureOutputDto>(taskPicture.Result);
                }
                throw new Exception("_birdsPictureConverter.UpdatePictureAsync fail.");
            }
            else
            {
                return null;
            }
        }
    }
}
